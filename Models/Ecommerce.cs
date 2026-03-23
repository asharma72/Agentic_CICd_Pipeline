using System.ComponentModel.DataAnnotations;

namespace Ecommerce.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product description is required")]
        [StringLength(500, ErrorMessage = "Product description cannot be longer than 500 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Product price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than zero")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Product quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product quantity must be greater than zero")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Product category is required")]
        [StringLength(100, ErrorMessage = "Product category cannot be longer than 100 characters")]
        public string Category { get; set; }
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Order total is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Order total must be greater than zero")]
        public decimal Total { get; set; }
        [Required(ErrorMessage = "Order status is required")]
        [StringLength(50, ErrorMessage = "Order status cannot be longer than 50 characters")]
        public string Status { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Order item product id is required")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Order item quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Order item quantity must be greater than zero")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Order item price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Order item price must be greater than zero")]
        public decimal Price { get; set; }
        public Order Order { get; set; }
    }

    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Customer name cannot be longer than 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Customer phone number is required")]
        [StringLength(20, ErrorMessage = "Customer phone number cannot be longer than 20 characters")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Customer address is required")]
        [StringLength(200, ErrorMessage = "Customer address cannot be longer than 200 characters")]
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}