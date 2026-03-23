using Ecommerce.API.Models;
using Ecommerce.API.Repositories;
using Ecommerce.API.Services;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using FluentAssertions;

namespace Ecommerce.API.Tests.Services
{
    public class EcommerceServiceTests
    {
        private readonly Mock<IEcommerceRepository> _repositoryMock;
        private readonly EcommerceService _service;

        public EcommerceServiceTests()
        {
            _repositoryMock = new Mock<IEcommerceRepository>();
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
        public async Task GetAll_ShouldReturnList_WhenItemsExist()
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
            result.Should().BeOfType<Product>();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedItem_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Product>();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Create_ShouldThrowArgumentException_WhenInvalidId(int id)
        {
            // Arrange
            var product = new Product { Id = id, Name = "Product 1" };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(product));
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedItem_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Product>();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Update_ShouldThrowArgumentException_WhenInvalidId(int id)
        {
            // Arrange
            var product = new Product { Id = id, Name = "Product 1" };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(product));
        }

        [Fact]
        public async Task Delete_ShouldNotThrow_WhenValidId()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(1));

            // Act and Assert
            await _service.DeleteAsync(1);
        }

        [Fact]
        public async Task Delete_ShouldThrowArgumentException_WhenInvalidId()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteAsync(0));
        }
    }
}