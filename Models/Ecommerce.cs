using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name should not exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        [StringLength(500, ErrorMessage = "Product description should not exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price should be a positive number")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product quantity should be a positive integer")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product category is required")]
        [StringLength(50, ErrorMessage = "Product category should not exceed 50 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Product image URL is required")]
        [StringLength(200, ErrorMessage = "Product image URL should not exceed 200 characters")]
        public string ImageUrl { get; set; }
    }
}