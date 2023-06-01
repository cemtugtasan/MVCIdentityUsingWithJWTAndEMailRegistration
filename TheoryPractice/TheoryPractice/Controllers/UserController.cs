using AppUser.Management.Service.Models;
using AppUser.Management.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TheoryPractice.Models.Authentication;
using TheoryPractice.Models.Authentication.SignIn;
using TheoryPractice.Models.Authentication.SignUp;

namespace TheoryPractice.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityUser> _roleManager;
		//IConfigiration ile appsettings jsn dosyasına ulaşmamızı sağlıyor.
		private readonly IConfiguration _configuration;
		private readonly IEmailService _emailService;

		public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityUser> roleManager,IConfiguration configuration,IEmailService emailService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_emailService= emailService;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp([FromForm] SignUpAppUser appUser, string role = "OYUNCU")
		{
			//Kayıt olabilmesi için benzersiz bir username ve e mail adresi olması gerekir.
			var userByName = await _userManager.FindByNameAsync(appUser.Username);
			var userByEmail = await _userManager.FindByEmailAsync(appUser.Email);
			if (userByName != null || userByEmail != null)
			{
				//oluşturduğumuz response classından aldık.
				return StatusCode(StatusCodes.Status403Forbidden, new AppResponse() { status = "Hata Oluştu!", statusMessage = "Bu username ya da mail adresine sahip kullanıcı sistemde var." });
			}
			else
			{
				var userRole = await _roleManager.FindByNameAsync(role);
				if (userRole != null)
				{
					IdentityUser eklenecekKullanıcı = new()
					{
						UserName = appUser.Username,
						SecurityStamp = Guid.NewGuid().ToString(),
					};
					var eklemeSonucu = await _userManager.CreateAsync(eklenecekKullanıcı, appUser.Password);

					if (eklemeSonucu.Succeeded)
					{
						await _userManager.AddToRoleAsync(eklenecekKullanıcı, role);
					}
					else
					{
						return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse()
						{
							status = "Hata oluştu!",
							statusMessage = "Kullanıcı sisteme kayıt edilirken bir hata oluştu."
						});
					}

				}
				return RedirectToAction("Index");
			}
		}
		public IActionResult SignIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignIn([FromForm] SignInAppUser appUser)
		{
			IActionResult viewResult;
			var userByName = await _userManager.FindByNameAsync(appUser.UserName);
			if (userByName != null)
			{
				if (await _userManager.CheckPasswordAsync(userByName, appUser.Password))
				{
					//Token Generation
					//Token Display
					var payload = new List<Claim>() { new Claim(ClaimTypes.Name, appUser.UserName), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };

					//3 saat 15 dk sonra yok olacak bir jwt üreteceğiz.
					var jwtToken = GetToken(payload, 3, 15);
					var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtToken);
					
					
					viewResult = Ok(new { token=tokenStr, expires=jwtToken.ValidTo });
				}
				else
				{
					viewResult = StatusCode(StatusCodes.Status406NotAcceptable, new AppResponse()
					{ status = "Hata!", statusMessage = "Sistemde Böyle Bir Kullanıcı Adı Bulunmamaktadır!" });
				}
			}
			else
			{
				viewResult = StatusCode(StatusCodes.Status406NotAcceptable, new AppResponse()
				{ status = "Hata!", statusMessage = "Sistemde Böyle Bir Kullanıcı Adı Bulunmamaktadır!" });
			}
			return View();
		}

		private JwtSecurityToken GetToken(List<Claim> payload, int hours, int minutes)
		{
			//Sunucu tarafındaki JWT:Secret
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT")["Secret"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				//Belirlenen saat ve dakika sonrasında token sonlanacak.
				expires: DateTime.UtcNow.AddHours(hours).AddMinutes(minutes),
				claims: payload,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				) ;
			return token;
		}
		public IActionResult SendMail()
		{
			var emailMessage = new MailMessage(new Dictionary<string, string>() { 
				{ "Fatih Kaan Açıkgöz 1", "fatihkaanacikgoz@gmail.com" }, 
				{ "Fatih Kaan Açıkgöz 2", "fatihimin3406@gmail.com" } }, 
				"İlk mail denemesi başlığı", "<h1>Mantığının anlaşılması gereken hadiseler</h1><ol><li>OOP Mantığı</li><li>SOLID prensipleri</li></ol><p>Yazan: <b>Fatih Kaan Açıkgöz</b></p>");

			_emailService.SendMail(emailMessage);

			return StatusCode(StatusCodes.Status200OK, new AppResponse() { status = "Başarılı", statusMessage = "Email başarıyla gönderildi!" });
		}
	}
}
