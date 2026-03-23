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
        public async Task GetAll_ShouldReturn200_WithNoItems()
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
            var ecommerce = new Ecommerce { Id = id };
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(ecommerce);
            var result = await _controller.GetById(id) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(ecommerce);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_ShouldReturn404_WithNoItem(int id)
        {
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.GetById(id) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Post_ShouldReturn201_WithItem()
        {
            var ecommerce = new Ecommerce();
            _mockService.Setup(s => s.CreateAsync(ecommerce))
                .ReturnsAsync(ecommerce);
            var result = await _controller.Post(ecommerce) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task Post_ShouldReturn400_WithInvalidItem()
        {
            var ecommerce = new Ecommerce();
            _controller.ModelState.AddModelError("Test", "Test");
            var result = await _controller.Post(ecommerce) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Put_ShouldReturn200_WithItem(int id)
        {
            var ecommerce = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(ecommerce))
                .ReturnsAsync(ecommerce);
            var result = await _controller.Put(id, ecommerce) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be(ecommerce);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Put_ShouldReturn404_WithNoItem(int id)
        {
            var ecommerce = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(ecommerce))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Put(id, ecommerce) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Put_ShouldReturn400_WithInvalidItem()
        {
            var ecommerce = new Ecommerce();
            _controller.ModelState.AddModelError("Test", "Test");
            var result = await _controller.Put(1, ecommerce) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Delete_ShouldReturn200_WithItem(int id)
        {
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(true);
            var result = await _controller.Delete(id) as OkResult;
            result?.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_ShouldReturn404_WithNoItem(int id)
        {
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(false);
            var result = await _controller.Delete(id) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}