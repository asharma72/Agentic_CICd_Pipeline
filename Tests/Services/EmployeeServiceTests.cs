using Employee.API.Services;
using Employee.API.Models;
using Employee.API.Repositories;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Employee.API.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _repositoryMock;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _repositoryMock = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoItems()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnList_WhenItemsExist()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe" },
                new Employee { Id = 2, Name = "Jane Doe" }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Employee)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee_WhenFound()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John Doe" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(employee);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Create_ShouldThrowArgumentNullException_WhenInputIsNull(string name)
        {
            // Arrange
            var employee = new Employee { Name = name };

            // Act and Assert
            await _service.Invoking(s => s.CreateAsync(employee)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedEmployee()
        {
            // Arrange
            var employee = new Employee { Name = "John Doe" };
            _repositoryMock.Setup(r => r.CreateAsync(employee)).ReturnsAsync(employee);

            // Act
            var result = await _service.CreateAsync(employee);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(employee);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Update_ShouldThrowArgumentNullException_WhenInputIsNull(string name)
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = name };

            // Act and Assert
            await _service.Invoking(s => s.UpdateAsync(employee)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedEmployee()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John Doe" };
            _repositoryMock.Setup(r => r.UpdateAsync(employee)).ReturnsAsync(employee);

            // Act
            var result = await _service.UpdateAsync(employee);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(employee);
        }

        [Fact]
        public async Task Delete_ShouldThrowArgumentNullException_WhenIdIsZero()
        {
            // Act and Assert
            await _service.Invoking(s => s.DeleteAsync(0)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenNotFound()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }
    }
}