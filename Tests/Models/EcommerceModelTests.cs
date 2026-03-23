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
        public void Product_ValidModel_ModelIsValid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product",
                Description = "Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Product_InvalidId_ModelIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 0,
                Name = "Product",
                Description = "Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_InvalidName_ModelIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = string.Empty,
                Description = "Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_InvalidDescription_ModelIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product",
                Description = new string('a', 501),
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_InvalidPrice_ModelIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product",
                Description = "Description",
                Price = -10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_InvalidQuantity_ModelIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product",
                Description = "Description",
                Price = 10.99m,
                Quantity = -10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}