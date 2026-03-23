using Ecommerce.API.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Ecommerce.API.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_ValidModel_PassesValidation()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Assert
            Validator.TryValidateObject(product, context, results);
            results.Should().BeEmpty();
        }

        [Fact]
        public void Product_InvalidName_ThrowsValidationException()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "", // empty name
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Assert
            Action act = () => Validator.TryValidateObject(product, context, results);
            act.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Product_Name_ExceedsMaxLength_ThrowsValidationException()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('a', 51), // exceeds max length of 50
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Assert
            Action act = () => Validator.TryValidateObject(product, context, results);
            act.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Product_Price_OutOfRange_ThrowsValidationException()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = -10.99m, // out of range
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Assert
            Action act = () => Validator.TryValidateObject(product, context, results);
            act.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Product_Quantity_OutOfRange_ThrowsValidationException()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = -10 // out of range
            };

            // Act
            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Assert
            Action act = () => Validator.TryValidateObject(product, context, results);
            act.ShouldThrow<ValidationException>();
        }
    }
}