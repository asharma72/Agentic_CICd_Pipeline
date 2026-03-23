using Employee.API.Models;
using FluentAssertions;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Employee.API.Tests.Models
{
    public class EmployeeModelTests
    {
        [Fact]
        public void ValidateEmployeeModel_RequiredFieldsValid_ModelIsValid()
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                Salary = 50000
            };

            // Act
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(employee, context, results);

            // Assert
            isValid.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact]
        public void ValidateEmployeeModel_RequiredFieldsMissing_ModelIsInvalid()
        {
            // Arrange
            var employee = new Employee();

            // Act
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(employee, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
            results.Should().Contain(r => r.ErrorMessage.Contains("FirstName"));
            results.Should().Contain(r => r.ErrorMessage.Contains("LastName"));
            results.Should().Contain(r => r.ErrorMessage.Contains("Age"));
            results.Should().Contain(r => r.ErrorMessage.Contains("Salary"));
        }

        [Fact]
        public void ValidateEmployeeModel_StringLengthValid_ModelIsValid()
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(employee, context, results);

            // Assert
            isValid.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact]
        public void ValidateEmployeeModel_StringLengthExceedsMaxLength_ModelIsInvalid()
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = new string('a', 51),
                LastName = new string('b', 51)
            };

            // Act
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(employee, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
            results.Should().Contain(r => r.ErrorMessage.Contains("FirstName"));
            results.Should().Contain(r => r.ErrorMessage.Contains("LastName"));
        }

        [Fact]
        public void ValidateEmployeeModel_RangeConstraintsValid_ModelIsValid()
        {
            // Arrange
            var employee = new Employee
            {
                Age = 30,
                Salary = 50000
            };

            // Act
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(employee, context, results);

            // Assert
            isValid.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact]
        public void ValidateEmployeeModel_RangeConstraintsExceedsMaxValue_ModelIsInvalid()
        {
            // Arrange
            var employee = new Employee
            {
                Age = 101,
                Salary = 200000
            };

            // Act
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(employee, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
            results.Should().Contain(r => r.ErrorMessage.Contains("Age"));
            results.Should().Contain(r => r.ErrorMessage.Contains("Salary"));
        }

        [Fact]
        public void ValidateEmployeeModel_RangeConstraintsBelowMinValue_ModelIsInvalid()
        {
            // Arrange
            var employee = new Employee
            {
                Age = 17,
                Salary = -1000
            };

            // Act
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(employee, context, results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().NotBeEmpty();
            results.Should().Contain(r => r.ErrorMessage.Contains("Age"));
            results.Should().Contain(r => r.ErrorMessage.Contains("Salary"));
        }
    }
}