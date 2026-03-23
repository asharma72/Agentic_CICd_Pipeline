using Ecommerce.API.Controllers;
using Ecommerce.API.Models;
using Ecommerce.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

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
        public async Task GetAll_ShouldReturn200_WithEmptyList()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Ecommerce>());
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetById_ShouldReturn200_WithItem()
        {
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(item);
            var result = await _controller.GetById(1) as OkObjectResult;
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

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_ShouldReturn400_WithInvalidId(int id)
        {
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.GetById(id) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Post_ShouldReturn201_WithCreatedItem()
        {
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);
            var result = await _controller.Post(new Ecommerce()) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
            result?.Value.Should().Be(item);
        }

        [Fact]
        public async Task Post_ShouldReturn400_WithInvalidItem()
        {
            _controller.ModelState.AddModelError("Item", "Invalid item");
            var result = await _controller.Post(new Ecommerce()) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Put_ShouldReturn200_WithUpdatedItem()
        {
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);
            var result = await _controller.Put(1, new Ecommerce()) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(item);
        }

        [Fact]
        public async Task Put_ShouldReturn404_WithInvalidId()
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Put(1, new Ecommerce()) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Put_ShouldReturn400_WithInvalidId(int id)
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Put(id, new Ecommerce()) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Delete_ShouldReturn200_WithDeletedItem()
        {
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(item);
            var result = await _controller.Delete(1) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(item);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithInvalidId()
        {
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Delete(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_ShouldReturn400_WithInvalidId(int id)
        {
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Delete(id) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }
    }
}