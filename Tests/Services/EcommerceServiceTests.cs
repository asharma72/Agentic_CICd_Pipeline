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
        public async Task GetAll_ShouldReturnAllItems_WhenItemsExist()
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
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            // Act and Assert
            await _service.Invoking(s => s.CreateAsync(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedProduct_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            // Act and Assert
            await _service.Invoking(s => s.UpdateAsync(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedProduct_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task Delete_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            // Act and Assert
            await _service.Invoking(s => s.DeleteAsync(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.DeleteAsync(product)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(product);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_ShouldReturnNull_WhenIdIsInvalid(int id)
        {
            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.Should().BeNull();
        }
    }
}