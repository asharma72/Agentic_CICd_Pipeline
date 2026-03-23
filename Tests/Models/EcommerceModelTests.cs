using Ecommerce.API.Models;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Ecommerce.API.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_ValidModel_IsValid()
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
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Product_InvalidId_IsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 0,
                Name = "Test Product",
                Price = 10.99m,
                Description = "This is a test product"
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Contain("Id");
        }

        [Fact]
        public void Product_MissingName_IsInvalid()
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
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Contain("Name");
        }

        [Fact]
        public void Product_NameTooLong_IsInvalid()
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
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Contain("Name");
        }

        [Fact]
        public void Product_InvalidPrice_IsInvalid()
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
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Contain("Price");
        }

        [Fact]
        public void Product_MissingDescription_IsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.99m
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Contain("Description");
        }

        [Fact]
        public void Product_DescriptionTooLong_IsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.99m,
                Description = new string('a', 501)
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Contain("Description");
        }
    }
}