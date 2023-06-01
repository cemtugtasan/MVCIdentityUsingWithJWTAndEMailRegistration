using System.ComponentModel.DataAnnotations;

namespace TheoryPractice.Models.Authentication.SignIn
{
	public class SignInAppUser
	{
		[Required(ErrorMessage = "Bu bilginin girilmesi zorunludur.")]
		public string? UserName { get; set; }
		[Required(ErrorMessage = "Bu bilginin girilmesi zorunludur.")]
		[DataType(DataType.Password)]
		public string? Password { get; set; }
	}
}
