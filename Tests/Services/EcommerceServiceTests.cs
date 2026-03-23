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
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly EcommerceService _service;

        public EcommerceServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _service = new EcommerceService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoItems()
        {
            // Arrange
            _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnItems_WhenItemsExist()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Product1" } };
            _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_ShouldReturnNull_WhenInvalidId(int id)
        {
            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Product)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnItem_WhenFound()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _productRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedItem_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _productRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(new Product { Name = "Product1" });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null));
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedItem_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(new Product { Id = 1, Name = "Product1" });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));
        }

        [Fact]
        public async Task Delete_ShouldNotThrow_WhenValidId()
        {
            // Arrange
            _productRepositoryMock.Setup(x => x.DeleteAsync(1));

            // Act and Assert
            await _service.DeleteAsync(1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_ShouldNotThrow_WhenInvalidId(int id)
        {
            // Act and Assert
            await _service.DeleteAsync(id);
        }
    }
}