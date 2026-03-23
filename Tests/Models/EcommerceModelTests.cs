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
        public void Product_Model_Validation_ValidModel_IsValid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "This is a test product",
                Price = 10.99m,
                Quantity = 5
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
        public void Product_Model_Validation_RequiredField_Name_IsRequired()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Description = "This is a test product",
                Price = 10.99m,
                Quantity = 5
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The Name field is required.");
        }

        [Fact]
        public void Product_Model_Validation_RequiredField_Description_IsRequired()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.99m,
                Quantity = 5
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The Description field is required.");
        }

        [Fact]
        public void Product_Model_Validation_StringLength_Name_MaxLength()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('a', 51),
                Description = "This is a test product",
                Price = 10.99m,
                Quantity = 5
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The field Name must be a string with a maximum length of 50.");
        }

        [Fact]
        public void Product_Model_Validation_StringLength_Description_MaxLength()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = new string('a', 201),
                Price = 10.99m,
                Quantity = 5
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The field Description must be a string with a maximum length of 200.");
        }

        [Fact]
        public void Product_Model_Validation_RangeConstraint_Price_MinValue()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "This is a test product",
                Price = -1.00m,
                Quantity = 5
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The field Price must be between 0 and 100.");
        }

        [Fact]
        public void Product_Model_Validation_RangeConstraint_Price_MaxValue()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "This is a test product",
                Price = 101.00m,
                Quantity = 5
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The field Price must be between 0 and 100.");
        }

        [Fact]
        public void Product_Model_Validation_RangeConstraint_Quantity_MinValue()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "This is a test product",
                Price = 10.99m,
                Quantity = -1
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The field Quantity must be between 0 and 100.");
        }

        [Fact]
        public void Product_Model_Validation_RangeConstraint_Quantity_MaxValue()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "This is a test product",
                Price = 10.99m,
                Quantity = 101
            };

            // Act
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().HaveCount(1);
            results[0].ErrorMessage.Should().Be("The field Quantity must be between 0 and 100.");
        }
    }
}