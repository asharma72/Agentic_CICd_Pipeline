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
            result?.Value.Should().BeEquivalentTo(new List<Ecommerce>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ShouldReturn200_WithItem(int id)
        {
            // Arrange
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
            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Post_ShouldReturn201_WithItem()
        {
            // Arrange
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Post(item) as CreatedAtActionResult;

            // Assert
            result?.StatusCode.Should().Be(201);
            result?.RouteValues.Should().ContainKey("id");
            result?.RouteValues["id"].Should().Be(item.Id);
        }

        [Fact]
        public async Task Post_ShouldReturn400_WithInvalidItem()
        {
            // Arrange
            var item = new Ecommerce { Id = 0 }; // Invalid item
            _controller.ModelState.AddModelError("Id", "Required");

            // Act
            var result = await _controller.Post(item);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Put_ShouldReturn200_WithItem(int id)
        {
            // Arrange
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(id, It.IsAny<Ecommerce>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Put(id, item) as OkResult;

            // Assert
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Put_ShouldReturn404_WithNoItem()
        {
            // Arrange
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(1, It.IsAny<Ecommerce>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Put(1, item);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Put_ShouldReturn400_WithInvalidItem()
        {
            // Arrange
            var item = new Ecommerce { Id = 0 }; // Invalid item
            _controller.ModelState.AddModelError("Id", "Required");

            // Act
            var result = await _controller.Put(1, item);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Delete_ShouldReturn200_WithItem(int id)
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id) as OkResult;

            // Assert
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithNoItem()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}