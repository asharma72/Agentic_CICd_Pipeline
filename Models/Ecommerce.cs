using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Product price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product category is required")]
        [StringLength(50, ErrorMessage = "Product category cannot exceed 50 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Product image is required")]
        [StringLength(200, ErrorMessage = "Product image cannot exceed 200 characters")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Product stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Product stock must be greater than or equal to 0")]
        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Order customer is required")]
        [StringLength(100, ErrorMessage = "Order customer cannot exceed 100 characters")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Order total is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Order total must be greater than or equal to 0")]
        public decimal Total { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Order item order is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Order item product is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Order item quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Order item quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Order item price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Order item price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Customer email is required")]
        [StringLength(100, ErrorMessage = "Customer email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Customer phone is required")]
        [StringLength(20, ErrorMessage = "Customer phone cannot exceed 20 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Customer address is required")]
        [StringLength(200, ErrorMessage = "Customer address cannot exceed 200 characters")]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}