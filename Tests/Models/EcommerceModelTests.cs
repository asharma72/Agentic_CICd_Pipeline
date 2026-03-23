using Ecommerce.API.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
                Description = "This is a test product",
                Price = 19.99m,
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
        public void Product_WithMissingRequiredField_Name_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Description = "This is a test product",
                Price = 19.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage == "The Name field is required.");
        }

        [Fact]
        public void Product_WithMissingRequiredField_Description_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 19.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage == "The Description field is required.");
        }

        [Fact]
        public void Product_WithNameExceedingMaxLength_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('a', 51),
                Description = "This is a test product",
                Price = 19.99m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage == "The field Name must be a string with a maximum length of 50.");
        }

        [Fact]
        public void Product_WithPriceOutOfRange_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "This is a test product",
                Price = -1.00m,
                Quantity = 10
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage == "The field Price must be between 0 and 1000.");
        }

        [Fact]
        public void Product_WithQuantityOutOfRange_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "This is a test product",
                Price = 19.99m,
                Quantity = -1
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage == "The field Quantity must be between 0 and 1000.");
        }
    }
}