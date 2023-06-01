using AppUser.Management.Service.Configurations;
using AppUser.Management.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheoryPractice.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//database e her yerden tek instance ile ula�mak i�in burada newliyoruz
builder.Services.AddDbContext<NorthwindContext>((opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("HomeNorthwind"));
}));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
{
	opt.Password.RequiredLength = 1;
	opt.Password.RequireDigit = false;
	opt.Password.RequireLowercase= false;
	opt.Password.RequireUppercase= false;
	opt.Password.RequireNonAlphanumeric= false;
}).AddEntityFrameworkStores<NorthwindContext>().AddDefaultTokenProviders();

//Sisteme giri� yapma i�lemleri kurallar�.
//Authentication i�leminin jwt arac�l��� ile olaca��n�n bilgisini veriyoruz.
builder.Services.AddAuthentication(opt =>
{
	//token kullan�m�n� ve kontrol�n� sa�layan mekanizma
	opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});

//json i�erisindeki de�erleri senin olu�turdu�um emailconfig s�n�f�n�n de�erlerine e�itleniyor ve singleton olarak ekleniyor.
builder.Services.AddSingleton(builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>());
builder.Services.AddScoped<IEmailService, EMailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
