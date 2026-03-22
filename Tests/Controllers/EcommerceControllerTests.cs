using Ecommerce.API.Controllers;
using Ecommerce.API.Services;
using Ecommerce.API.Models;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Tests.Controllers
{
    public class EcommerceControllerTests
    {
        private readonly Mock<IEcommerceService> _mockService;
        private readonly EcommerceController _controller;

        public EcommerceControllerTests()
        {
            _mockService = new Mock<IEcommerceService>();
            _controller  = new EcommerceController(_mockService.Object);
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
        public async Task GetAll_ShouldReturn200_WithEmptyList()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Ecommerce>());
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ShouldReturn200_WithItem(int id)
        {
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(item);
            var result = await _controller.GetById(id) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(item);
        }

        [Fact]
        public async Task GetById_ShouldReturn404_WithInvalidId()
        {
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.GetById(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Post_ShouldReturn201_WithNewItem()
        {
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);
            var result = await _controller.Post(item) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
            result?.Value.Should().Be(item);
        }

        [Fact]
        public async Task Post_ShouldReturn400_WithInvalidItem()
        {
            var controllerContext = new ControllerContext();
            controllerContext.ModelState.AddModelError("key", "error message");
            _controller.ControllerContext = controllerContext;
            var result = await _controller.Post(new Ecommerce()) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Put_ShouldReturn200_WithUpdatedItem()
        {
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);
            var result = await _controller.Put(item) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(item);
        }

        [Fact]
        public async Task Put_ShouldReturn404_WithInvalidId()
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Put(new Ecommerce()) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Delete_ShouldReturn200_WithDeletedItem()
        {
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var result = await _controller.Delete(1) as OkResult;
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithInvalidId()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var result = await _controller.Delete(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}