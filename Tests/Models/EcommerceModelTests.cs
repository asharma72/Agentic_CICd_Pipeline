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
        public void Product_ValidModel_ModelStateIsValid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product1",
                Description = "This is a product",
                Price = 10.99m,
                Quantity = 10
            };

            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Product_MissingId_ModelStateIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product1",
                Description = "This is a product",
                Price = 10.99m,
                Quantity = 10
            };

            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_MissingName_ModelStateIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Description = "This is a product",
                Price = 10.99m,
                Quantity = 10
            };

            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_NameTooLong_ModelStateIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('a', 101),
                Description = "This is a product",
                Price = 10.99m,
                Quantity = 10
            };

            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_PriceOutOfRange_ModelStateIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product1",
                Description = "This is a product",
                Price = -1.00m,
                Quantity = 10
            };

            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Product_QuantityOutOfRange_ModelStateIsInvalid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product1",
                Description = "This is a product",
                Price = 10.99m,
                Quantity = -1
            };

            var context = new ValidationContext(product);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(product, context, results);

            // Assert
            isValid.Should().BeFalse();
        }
    }

    public class CustomerTests
    {
        [Fact]
        public void Customer_ValidModel_ModelStateIsValid()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var context = new ValidationContext(customer);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(customer, context, results);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Customer_MissingEmail_ModelStateIsInvalid()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe"
            };

            var context = new ValidationContext(customer);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(customer, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Customer_InvalidEmail_ModelStateIsInvalid()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email"
            };

            var context = new ValidationContext(customer);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(customer, context, results);

            // Assert
            isValid.Should().BeFalse();
        }
    }

    public class OrderTests
    {
        [Fact]
        public void Order_ValidModel_ModelStateIsValid()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Total = 100.00m
            };

            var context = new ValidationContext(order);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(order, context, results);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Order_MissingCustomerId_ModelStateIsInvalid()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                OrderDate = DateTime.Now,
                Total = 100.00m
            };

            var context = new ValidationContext(order);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(order, context, results);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Order_TotalOutOfRange_ModelStateIsInvalid()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Total = -100.00m
            };

            var context = new ValidationContext(order);
            var results = new ValidationResult[0];

            // Act
            var isValid = Validator.TryValidateObject(order, context, results);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}