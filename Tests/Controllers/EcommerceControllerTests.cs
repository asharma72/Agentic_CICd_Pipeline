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
            (result?.Value as List<Ecommerce>).Should().BeEmpty();
        }

        [Fact]
        public async Task GetById_ShouldReturn200_WithItem()
        {
            var ecommerce = new Ecommerce { Id = 1, Name = "Test" };
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(ecommerce);
            var result = await _controller.GetById(1) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeOfType<Ecommerce>();
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
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ShouldReturn404_WithInvalidIds(int id)
        {
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.GetById(id) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Post_ShouldReturn201_WithNewItem()
        {
            var ecommerce = new Ecommerce { Name = "Test" };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(ecommerce);
            var result = await _controller.Post(ecommerce) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
            result?.Value.Should().BeOfType<Ecommerce>();
        }

        [Fact]
        public async Task Post_ShouldReturn400_WithInvalidItem()
        {
            var ecommerce = new Ecommerce();
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Post(ecommerce) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Put_ShouldReturn200_WithUpdatedItem()
        {
            var ecommerce = new Ecommerce { Id = 1, Name = "Test" };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(ecommerce);
            var result = await _controller.Put(1, ecommerce) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeOfType<Ecommerce>();
        }

        [Fact]
        public async Task Put_ShouldReturn404_WithInvalidId()
        {
            var ecommerce = new Ecommerce { Id = 1, Name = "Test" };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Put(1, ecommerce) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Delete_ShouldReturn200_WithDeletedItem()
        {
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(true);
            var result = await _controller.Delete(1) as OkResult;
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithInvalidId()
        {
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(false);
            var result = await _controller.Delete(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}