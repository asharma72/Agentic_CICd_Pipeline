using Ecommerce.API.Models;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Ecommerce.API.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_ValidModel_NoValidationErrors()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Price = 10.99m,
                Description = "Product Description"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Product_InvalidId_ValidationError()
        {
            // Arrange
            var product = new Product
            {
                Id = 0,
                Name = "Product Name",
                Price = 10.99m,
                Description = "Product Description"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
        }

        [Fact]
        public void Product_InvalidName_ValidationError()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = string.Empty,
                Price = 10.99m,
                Description = "Product Description"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
        }

        [Fact]
        public void Product_InvalidPrice_ValidationError()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Price = -10.99m,
                Description = "Product Description"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
        }

        [Fact]
        public void Product_DescriptionTooLong_ValidationError()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product Name",
                Price = 10.99m,
                Description = new string('a', 501)
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
        }
    }
}