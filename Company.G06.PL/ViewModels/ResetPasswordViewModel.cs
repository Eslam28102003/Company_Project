using System.ComponentModel.DataAnnotations;

namespace Company.G06.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		
		public string Password { get; set; }


		[Required(ErrorMessage = "ConfirmPassword Is Required")]
		[Compare(nameof(Password), ErrorMessage = "Confirmed Password Does Not Match Password !!")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
