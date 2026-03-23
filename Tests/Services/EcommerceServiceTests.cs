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
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());
            var result = await _service.GetAllAsync();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnItems_WhenItemsExist()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            var result = await _service.GetAllAsync();
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);
            var result = await _service.GetByIdAsync(999);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnItem_WhenItemExists()
        {
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            var result = await _service.GetByIdAsync(1);
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenNullInput()
        {
            await _service.Invoking(s => s.CreateAsync(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedItem_WhenValidInput()
        {
            var product = new Product { Name = "Product 1" };
            _repositoryMock.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);
            var result = await _service.CreateAsync(product);
            result.Should().NotBeNull();
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentNullException_WhenNullInput()
        {
            await _service.Invoking(s => s.UpdateAsync(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedItem_WhenValidInput()
        {
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);
            var result = await _service.UpdateAsync(product);
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task Delete_ShouldThrowArgumentNullException_WhenNullInput()
        {
            await _service.Invoking(s => s.DeleteAsync(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Delete_ShouldNotThrow_WhenValidInput()
        {
            var product = new Product { Id = 1, Name = "Product 1" };
            _repositoryMock.Setup(r => r.DeleteAsync(product));
            await _service.Invoking(s => s.DeleteAsync(product)).Should().NotThrow();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_ShouldReturnNull_WhenInvalidId(int id)
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product)null);
            var result = await _service.GetByIdAsync(id);
            result.Should().BeNull();
        }
    }
}