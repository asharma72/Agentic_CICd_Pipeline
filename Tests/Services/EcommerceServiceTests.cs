using Ecommerce.API.Services;
using Ecommerce.API.Models;
using Moq;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ecommerce.API.Tests.Services
{
    public class EcommerceServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly EcommerceService _service;

        public EcommerceServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _service = new EcommerceService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoItems()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

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
            var products = new List<Product> { new Product { Id = 1, Name = "Product1" }, new Product { Id = 2, Name = "Product2" } };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.FirstOrDefault(p => p.Id == 1).Should().NotBeNull();
            result.FirstOrDefault(p => p.Id == 2).Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnItem_WhenItemExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product1");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_ShouldThrowArgumentNullException_WhenInputIsNull(string productName)
        {
            // Arrange
            var product = new Product { Name = productName };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(product));
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedItem_WhenInputIsValid()
        {
            // Arrange
            var product = new Product { Name = "Product1" };
            _mockRepository.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Product1");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Update_ShouldThrowArgumentNullException_WhenInputIsNull(string productName)
        {
            // Arrange
            var product = new Product { Name = productName };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(product));
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedItem_WhenInputIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _mockRepository.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product1");
        }

        [Fact]
        public async Task Delete_ShouldThrowArgumentNullException_WhenIdIsZero()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.DeleteAsync(0));
        }

        [Fact]
        public async Task Delete_ShouldNotThrowException_WhenIdIsValid()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1));

            // Act and Assert
            await _service.DeleteAsync(1);
        }
    }
}