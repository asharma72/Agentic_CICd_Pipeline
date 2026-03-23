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
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults);

            // Assert
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Product_InvalidName_TooLong_NoValidationErrors()
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
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Name"));
        }

        [Fact]
        public void Product_InvalidPrice_OutOfRange_NoValidationErrors()
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
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Price"));
        }

        [Fact]
        public void Product_InvalidQuantity_OutOfRange_NoValidationErrors()
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
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Quantity"));
        }

        [Fact]
        public void Product_MissingRequiredField_NoValidationErrors()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Description = "Product Description",
                Price = 10.99m,
                Quantity = 10
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(r => r.MemberNames.Contains("Name"));
        }
    }
}