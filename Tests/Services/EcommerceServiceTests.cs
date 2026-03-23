using Ecommerce.API.Services;
using Ecommerce.API.Models;
using Ecommerce.API.Repositories;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.API.Tests.Services
{
    public class EcommerceServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly EcommerceService _service;

        public EcommerceServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
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
        public async Task GetAll_ShouldReturnAllItems_WhenItemsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" },
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
        public async Task GetById_ShouldReturnItem_WhenItemExists()
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Create_ShouldThrowArgumentNullException_WhenInputIsNull(string name)
        {
            // Arrange
            var product = new Product { Name = name };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(product));
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedItem_WhenInputIsValid()
        {
            // Arrange
            var product = new Product { Name = "Product 1" };
            _repositoryMock.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Update_ShouldThrowArgumentNullException_WhenInputIsNull(string name)
        {
            // Arrange
            var product = new Product { Name = name };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(product));
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedItem_WhenInputIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Delete_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.DeleteAsync(null));
        }

        [Fact]
        public async Task Delete_ShouldNotThrowException_WhenInputIsValid()
        {
            // Arrange
            var id = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(id));

            // Act and Assert
            await _service.DeleteAsync(id);
        }
    }
}