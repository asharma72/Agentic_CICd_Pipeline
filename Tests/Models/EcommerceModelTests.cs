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
        public void Product_ValidateModel_RequiredFieldsMissing_ValidationError()
        {
            // Arrange
            var product = new Product { Name = null, Price = null, Description = null };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(3);
        }

        [Fact]
        public void Product_ValidateModel_StringLength_ValidationError()
        {
            // Arrange
            var product = new Product { Name = new string('a', 51), Price = 10, Description = new string('a', 201) };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(2);
        }

        [Fact]
        public void Product_ValidateModel_RangeConstraints_ValidationError()
        {
            // Arrange
            var product = new Product { Price = -1 };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
        }

        [Fact]
        public void Product_ValidateModel_ValidModel_NoValidationError()
        {
            // Arrange
            var product = new Product { Name = "Product1", Price = 10, Description = "This is a product" };

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