using Ecommerce.API.Models;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Ecommerce.API.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_Model_Validation_RequiredFields()
        {
            // Arrange
            var product = new Product { Name = "", Description = "", Price = 0 };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(3);
            results.Select(r => r.ErrorMessage).Should().Contain("Name is required");
            results.Select(r => r.ErrorMessage).Should().Contain("Description is required");
            results.Select(r => r.ErrorMessage).Should().Contain("Price must be between 1 and 10000");
        }

        [Fact]
        public void Product_Model_Validation_StringLength()
        {
            // Arrange
            var product = new Product { Name = new string('a', 51), Description = new string('a', 201) };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(2);
            results.Select(r => r.ErrorMessage).Should().Contain("Name must be at most 50 characters");
            results.Select(r => r.ErrorMessage).Should().Contain("Description must be at most 200 characters");
        }

        [Fact]
        public void Product_Model_Validation_RangeConstraints()
        {
            // Arrange
            var product = new Product { Price = -1, Quantity = -1 };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(2);
            results.Select(r => r.ErrorMessage).Should().Contain("Price must be between 1 and 10000");
            results.Select(r => r.ErrorMessage).Should().Contain("Quantity must be between 1 and 1000");
        }

        [Fact]
        public void Product_Model_Validation_ValidData()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Description = "Test Description", Price = 10, Quantity = 5 };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeTrue();
            results.Should().BeEmpty();
        }
    }
}