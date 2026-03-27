using System.ComponentModel.DataAnnotations;

namespace E_commerce_PetShop.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username should have a minimum of 5 characters and a maximum of 50.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain symbols.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password should be a minimum of 5 and a maximum of 50.")]
        public string Password { get; set; }
    }
}
