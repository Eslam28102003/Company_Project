using System.ComponentModel.DataAnnotations;

namespace Company.G06.PL.ViewModels
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage = "UserName Is Required")]
        public string UserName { get; set; }
		[Required(ErrorMessage = "FirstName Is Required")]

		public string FirstName { get; set; }
		[Required(ErrorMessage = "LastName Is Required")]

		public string LastName { get; set; }
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email ")]

		public string Email { get; set; }

		[Required(ErrorMessage = "ConfirmEmail Is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email ")]
		[Compare(nameof(Email) , ErrorMessage ="Confirmed Email Not Match The Email !!")]
		public string ConfirmEmail { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		

		public string Password { get; set; }
		[Required(ErrorMessage = "ConfirmPassword Is Required")]
		[Compare(nameof(Password),ErrorMessage ="Confirmed Password Does Not Match Password !!")]
		[DataType(DataType.Password)]


		public string ConfirmPassword { get; set; }
		
	}
}
