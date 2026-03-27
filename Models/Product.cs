using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_PetShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Input price.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, 10000, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Url(ErrorMessage = "Invalid image URL.")]
        public string ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
