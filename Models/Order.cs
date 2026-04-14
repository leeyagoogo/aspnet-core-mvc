using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations;
using System;

namespace E_commerce_PetShop.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Completed = 3,
        Cancelled = 4
    }

    public class Order
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer name must be between 2 and 100 characters.")]
        [Display(Name = "Ordered By")]
        public string OrderedByName { get; set; }

        [Range(0, 10000, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderedAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999.99, ErrorMessage = "Total must be a non-negative value.")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
    }
}