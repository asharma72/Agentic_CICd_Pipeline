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
            // Arrange
            var items = new List<Ecommerce> { new Ecommerce(), new Ecommerce() };
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetAll_ShouldReturn200_WithNoItems()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Ecommerce>());

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task GetById_ShouldReturn200_WithItem()
        {
            // Arrange
            var id = 1;
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.GetById(id) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task GetById_ShouldReturn404_WithNoItem()
        {
            // Arrange
            var id = 1;
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.GetById(id) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_ShouldReturn201_WithItem()
        {
            // Arrange
            var item = new Ecommerce();
            _mockService.Setup(s => s.CreateAsync(item))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Create(item) as CreatedAtActionResult;

            // Assert
            result?.StatusCode.Should().Be(201);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Create_ShouldReturn400_WithInvalidItem()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid item");

            // Act
            var result = await _controller.Create(new Ecommerce()) as BadRequestObjectResult;

            // Assert
            result?.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Update_ShouldReturn200_WithItem(int id)
        {
            // Arrange
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(item))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Update(id, item) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Update_ShouldReturn404_WithNoItem()
        {
            // Arrange
            var id = 1;
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(item))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.Update(id, item) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Update_ShouldReturn400_WithInvalidItem()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid item");

            // Act
            var result = await _controller.Update(1, new Ecommerce()) as BadRequestObjectResult;

            // Assert
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Delete_ShouldReturn200_WithItem()
        {
            // Arrange
            var id = 1;
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Delete(id) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithNoItem()
        {
            // Arrange
            var id = 1;
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }
    }
}