using Ecommerce.API.Services;
using Ecommerce.API.Models;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.API.Tests.Services
{
    public class EcommerceServiceTests
    {
        private readonly Mock<IRepository> _repositoryMock;
        private readonly EcommerceService _service;

        public EcommerceServiceTests()
        {
            _repositoryMock = new Mock<IRepository>();
            _service = new EcommerceService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoItems()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfProducts_WhenItemsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

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
            _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnProduct_WhenFound()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedProduct_WhenValidInput()
        {
            // Arrange
            var product = new Product { Name = "Product 1" };
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await _service.Invoking(s => s.CreateAsync(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedProduct_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await _service.Invoking(s => s.UpdateAsync(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Delete_ShouldNotThrow_WhenValidId()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(1)).Verifiable();

            // Act and Assert
            await _service.DeleteAsync(1);
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_ShouldThrowArgumentException_WhenInvalidId(int id)
        {
            // Act and Assert
            await _service.Invoking(s => s.DeleteAsync(id)).Should().ThrowAsync<ArgumentException>();
        }
    }
}