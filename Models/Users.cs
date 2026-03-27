using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_PetShop.Models
{
    public class Users
    {
        [Key]

        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name should have a minimum of 2 letters and a maximum of 50.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email should be valid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username should have a minimum of 5 letters and a maximum of 50.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain symbols.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password should be a minimum of 5 and a maximum of 50.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
    }
}
