using System.ComponentModel.DataAnnotations;

namespace Company.G06.PL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email ")]

        public string Email { get; set; }
    }
}
