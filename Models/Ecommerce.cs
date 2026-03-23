using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.API.Models
{
    public class ProductModel
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
        [Range(0, 10000, ErrorMessage = "Product price must be between 0 and 10000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product quantity is required")]
        [Range(0, 1000, ErrorMessage = "Product quantity must be between 0 and 1000")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product category is required")]
        [StringLength(50, ErrorMessage = "Product category cannot be longer than 50 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Product image URL is required")]
        [StringLength(200, ErrorMessage = "Product image URL cannot be longer than 200 characters")]
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Order total is required")]
        [Range(0, 10000, ErrorMessage = "Order total must be between 0 and 10000")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Order status is required")]
        [StringLength(50, ErrorMessage = "Order status cannot be longer than 50 characters")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        public int CustomerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CustomerModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Customer name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Customer email is required")]
        [StringLength(100, ErrorMessage = "Customer email cannot be longer than 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Customer phone number is required")]
        [StringLength(20, ErrorMessage = "Customer phone number cannot be longer than 20 characters")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Customer address is required")]
        [StringLength(200, ErrorMessage = "Customer address cannot be longer than 200 characters")]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}