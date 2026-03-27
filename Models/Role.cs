using System.ComponentModel.DataAnnotations;

namespace E_commerce_PetShop.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string UserRole { get; set; }
    }
}
