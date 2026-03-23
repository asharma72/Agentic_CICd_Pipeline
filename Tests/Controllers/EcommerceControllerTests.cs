using Ecommerce.API.Controllers;
using Ecommerce.API.Models;
using Ecommerce.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
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
        public async Task GetAll_ShouldReturn200_WithNoItems()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Ecommerce>());
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetById_ShouldReturn200_WithItem(int id)
        {
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(new Ecommerce { Id = id });
            var result = await _controller.GetById(id) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetById_ShouldReturn404_WithNoItem()
        {
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.GetById(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_ShouldReturn201_WithNewItem()
        {
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(new Ecommerce { Id = 1 });
            var result = await _controller.Create(new Ecommerce()) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task Create_ShouldReturn400_WithInvalidItem()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Create(new Ecommerce()) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Update_ShouldReturn200_WithUpdatedItem()
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(new Ecommerce { Id = 1 });
            var result = await _controller.Update(new Ecommerce { Id = 1 }) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Update_ShouldReturn404_WithNoItem()
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync((Ecommerce)null);
            var result = await _controller.Update(new Ecommerce { Id = 1 }) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Update_ShouldReturn400_WithInvalidItem()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Update(new Ecommerce()) as BadRequestObjectResult;
            result?.StatusCode.Should().Be(400);
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
        public async Task Delete_ShouldReturn404_WithNoItem()
        {
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(false);
            var result = await _controller.Delete(1) as NotFoundResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}