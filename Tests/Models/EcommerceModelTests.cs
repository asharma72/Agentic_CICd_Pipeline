using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Ecommerce.API.Models;

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
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Product_InvalidId_FailsValidation()
        {
            // Arrange
            var product = new Product
            {
                Id = -1,
                Name = "Product Name",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
        }

        [Fact]
        public void Product_EmptyName_FailsValidation()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
        }

        [Fact]
        public void Product_NameExceedsMaxLength_FailsValidation()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('a', 51),
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
        }

        [Fact]
        public void Product_PriceOutOfRange_FailsValidation()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = -10.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
        }

        [Fact]
        public void Product_QuantityOutOfRange_FailsValidation()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Description = "Product Description",
                Price = 10.99m,
                Quantity = -10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
        }
    }
}