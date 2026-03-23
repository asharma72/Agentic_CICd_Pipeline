using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Product description cannot be longer than 500 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Category name cannot be longer than 100 characters")]
        public string Category { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Brand name cannot be longer than 100 characters")]
        public string Brand { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Image URL cannot be longer than 100 characters")]
        public string ImageUrl { get; set; }
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Customer name cannot be longer than 100 characters")]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Customer email cannot be longer than 100 characters")]
        public string CustomerEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Customer address cannot be longer than 100 characters")]
        public string CustomerAddress { get; set; }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Role cannot be longer than 100 characters")]
        public string Role { get; set; }
    }
}