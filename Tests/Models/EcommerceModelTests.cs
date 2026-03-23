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
        public void Product_Model_Validation_RequiredFields()
        {
            // Arrange
            var product = new Product
            {
                Name = null,
                Description = null,
                Price = 0,
                Quantity = 0
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(3);
        }

        [Fact]
        public void Product_Model_Validation_StringLength()
        {
            // Arrange
            var product = new Product
            {
                Name = new string('a', 51),
                Description = new string('a', 501),
                Price = 10,
                Quantity = 1
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(2);
        }

        [Fact]
        public void Product_Model_Validation_RangeConstraints()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = -1,
                Quantity = -1
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(2);
        }

        [Fact]
        public void Product_Model_Validation_ValidData()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10,
                Quantity = 1
            };

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