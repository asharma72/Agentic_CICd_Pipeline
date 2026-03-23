using Ecommerce.API.Services;
using Ecommerce.API.Models;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                new Product { Id = 1, Name = "Product1" },
                new Product { Id = 2, Name = "Product2" }
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
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnProduct_WhenFound()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product1");
        }

        [Theory]
        [InlineData(null)]
        public async Task Create_ShouldThrowArgumentNullException_WhenNullInput(Product product)
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(product));
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedProduct_WhenValidInput()
        {
            // Arrange
            var product = new Product { Name = "Product1" };
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Product1");
        }

        [Theory]
        [InlineData(null)]
        public async Task Update_ShouldThrowArgumentNullException_WhenNullInput(Product product)
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(product));
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedProduct_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product1");
        }

        [Fact]
        public async Task Delete_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.DeleteAsync(null));
        }

        [Fact]
        public async Task Delete_ShouldNotThrow_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<int>())).Verifiable();

            // Act and Assert
            await _service.DeleteAsync(product);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}