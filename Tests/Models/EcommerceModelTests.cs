using Ecommerce.API.Models;
using FluentAssertions;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Ecommerce.API.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_WithValidData_ShouldBeValid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.99m,
                Description = "This is a test product"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult();

            // Assert
            Validator.TryValidateObject(product, context, results, true);
            results.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void Product_WithMissingRequiredField_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Price = 10.99m,
                Description = "This is a test product"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult();

            // Assert
            Validator.TryValidateObject(product, context, results, true);
            results.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Product_WithNameExceedingMaxLength_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('a', 101),
                Price = 10.99m,
                Description = "This is a test product"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult();

            // Assert
            Validator.TryValidateObject(product, context, results, true);
            results.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Product_WithPriceOutOfRange_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = -10.99m,
                Description = "This is a test product"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult();

            // Assert
            Validator.TryValidateObject(product, context, results, true);
            results.Should().NotBe(ValidationResult.Success);
        }
    }

    public class Product
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [StringLength(500, MinimumLength = 1)]
        public string Description { get; set; }
    }
}