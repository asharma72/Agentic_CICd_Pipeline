using Ecommerce.API.Controllers;
using Ecommerce.API.Models;
using Ecommerce.API.Services;
using Moq;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using FluentAssertions;

namespace Ecommerce.API.Tests.Controllers
{
    public class EcommerceControllerTests
    {
        private readonly Mock<IEcommerceService> _mockService;
        private readonly EcommerceController _controller;

        public EcommerceControllerTests()
        {
            _mockService = new Mock<IEcommerceService>();
            _controller = new EcommerceController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturn200_WithItems()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Ecommerce>());
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAll_ShouldReturn404_WithNoItems()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync((List<Ecommerce>)null);
            var result = await _controller.GetAll() as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetById_ShouldReturn200_WithExistingItem()
        {
            var existingItem = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(existingItem);
            var result = await _controller.GetById(1) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(existingItem);
        }

        [Fact]
        public async Task GetById_ShouldReturn404_WithNonExistingItem()
        {
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.GetById(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_InvalidId_ShouldReturn400(int id)
        {
            var result = await _controller.GetById(id) as BadRequestResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Create_ShouldReturn201_WithNewlyCreatedItem()
        {
            var newItem = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(newItem);
            var result = await _controller.Create(newItem) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
            result?.Value.Should().Be(newItem);
        }

        [Fact]
        public async Task Create_InvalidItem_ShouldReturn400()
        {
            var result = await _controller.Create(new Ecommerce()) as BadRequestResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Update_ShouldReturn200_WithUpdatedItem()
        {
            var updatedItem = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(updatedItem);
            var result = await _controller.Update(updatedItem) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(updatedItem);
        }

        [Fact]
        public async Task Update_InvalidItem_ShouldReturn400()
        {
            var result = await _controller.Update(new Ecommerce()) as BadRequestResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Delete_ShouldReturn200_WithExistingItem()
        {
            var existingItem = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(existingItem);
            var result = await _controller.Delete(1) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(existingItem);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithNonExistingItem()
        {
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Delete(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}