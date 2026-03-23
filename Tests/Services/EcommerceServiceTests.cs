using Ecommerce.API.Services;
using Ecommerce.API.Models;
using Ecommerce.API.Repositories;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task GetAll_ShouldReturnProductList_WhenItemsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1" },
                new Product { Id = 2, Name = "Product2" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

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
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);

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
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Product>();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedProduct_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _mockRepository.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Product>();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null));
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedProduct_WhenValidInput()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _mockRepository.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Product>();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_WhenNullInput()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Product { Id = 1, Name = "Product1" });
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_ShouldThrowArgumentOutOfRangeException_WhenInvalidId(int id)
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.DeleteAsync(id));
        }
    }
}