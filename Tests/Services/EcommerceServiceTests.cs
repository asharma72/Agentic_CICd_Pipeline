using Ecommerce.API.Services;
using Ecommerce.API.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

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
        public async Task GetAll_ShouldReturnItems_WhenAvailable()
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
        public async Task GetById_ShouldReturnItem_WhenFound()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(product);
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null));
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedProduct_WhenValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(product);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Update_ShouldThrowArgumentException_WhenIdIsInvalid(int id)
        {
            // Arrange
            var product = new Product { Id = id, Name = "Product 1" };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(product));
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedProduct_WhenValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(product);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_ShouldThrowArgumentException_WhenIdIsInvalid(int id)
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteAsync(id));
        }

        [Fact]
        public async Task Delete_ShouldNotThrow_WhenIdIsValid()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(1)).Verifiable();

            // Act and Assert
            await _service.DeleteAsync(1);
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}