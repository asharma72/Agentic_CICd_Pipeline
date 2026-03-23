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
            var items = new List<Ecommerce> { new Ecommerce(), new Ecommerce() };
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(items);
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeOfType<List<Ecommerce>>();
            ((List<Ecommerce>)result?.Value).Count.Should().Be(2);
        }

        [Fact]
        public async Task GetAll_ShouldReturn200_WithoutItems()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Ecommerce>());
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeOfType<List<Ecommerce>>();
            ((List<Ecommerce>)result?.Value).Count.Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ValidId_ShouldReturn200(int id)
        {
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(item);
            var result = await _controller.GetById(id) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeOfType<Ecommerce>();
            ((Ecommerce)result?.Value).Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_InvalidId_ShouldReturn404(int id)
        {
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.GetById(id) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Post_ValidItem_ShouldReturn201()
        {
            var item = new Ecommerce();
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);
            var result = await _controller.Post(item) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task Post_InvalidItem_ShouldReturn400()
        {
            var item = new Ecommerce();
            _controller.ModelState.AddModelError("Error", "Invalid item");
            var result = await _controller.Post(item) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Put_ValidItemAndId_ShouldReturn200(int id)
        {
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(true);
            var result = await _controller.Put(id, item) as OkResult;
            result?.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Put_InvalidId_ShouldReturn404(int id)
        {
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(false);
            var result = await _controller.Put(id, item) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Delete_ValidId_ShouldReturn200(int id)
        {
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(true);
            var result = await _controller.Delete(id) as OkResult;
            result?.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_InvalidId_ShouldReturn404(int id)
        {
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(false);
            var result = await _controller.Delete(id) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}