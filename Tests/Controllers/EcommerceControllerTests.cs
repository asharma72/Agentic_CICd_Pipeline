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
            var items = new List<Ecommerce>
            {
                new Ecommerce { Id = 1, Name = "Item 1" },
                new Ecommerce { Id = 2, Name = "Item 2" }
            };
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetAll_ShouldReturn200_WithoutItems()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Ecommerce>());

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new List<Ecommerce>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ShouldReturn200_WithItem(int id)
        {
            // Arrange
            var item = new Ecommerce { Id = id, Name = $"Item {id}" };
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.GetById(id) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task GetById_ShouldReturn404_WithoutItem()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.GetById(1) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Post_ShouldReturn201_WithItem()
        {
            // Arrange
            var item = new Ecommerce { Id = 1, Name = "Item 1" };
            _mockService.Setup(s => s.CreateAsync(item))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Post(item) as CreatedAtActionResult;

            // Assert
            result?.StatusCode.Should().Be(201);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Post_ShouldReturn400_WithInvalidItem()
        {
            // Arrange
            var item = new Ecommerce { Name = "Item 1" };
            _controller.ModelState.AddModelError("Id", "Id is required");

            // Act
            var result = await _controller.Post(item) as BadRequestObjectResult;

            // Assert
            result?.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Put_ShouldReturn200_WithItem(int id)
        {
            // Arrange
            var item = new Ecommerce { Id = id, Name = $"Item {id}" };
            _mockService.Setup(s => s.UpdateAsync(item))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Put(id, item) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Put_ShouldReturn404_WithoutItem()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.Put(1, new Ecommerce { Id = 1, Name = "Item 1" }) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Put_ShouldReturn400_WithInvalidItem()
        {
            // Arrange
            var item = new Ecommerce { Name = "Item 1" };
            _controller.ModelState.AddModelError("Id", "Id is required");

            // Act
            var result = await _controller.Put(1, item) as BadRequestObjectResult;

            // Assert
            result?.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Delete_ShouldReturn200_WithItem(int id)
        {
            // Arrange
            var item = new Ecommerce { Id = id, Name = $"Item {id}" };
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id) as OkResult;

            // Assert
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithoutItem()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }
    }
}