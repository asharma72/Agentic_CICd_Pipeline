using Ecommerce.API.Controllers;
using Ecommerce.API.Models;
using Ecommerce.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task GetAll_ShouldReturn200_WithEmptyList()
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
        public async Task GetById_ShouldReturn404_WithNotFoundItem()
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
        public async Task Create_ShouldReturn201_WithCreatedItem()
        {
            // Arrange
            var item = new Ecommerce();
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
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
            var item = new Ecommerce();
            _controller.ModelState.AddModelError("Test", "Test error");

            // Act
            var result = await _controller.Create(item) as BadRequestObjectResult;

            // Assert
            result?.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Update_ShouldReturn200_WithUpdatedItem(int id)
        {
            // Arrange
            var item = new Ecommerce { Id = id };
            _mockService.Setup(s => s.UpdateAsync(id, It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Update(id, item) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Update_ShouldReturn404_WithNotFoundItem()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateAsync(1, It.IsAny<Ecommerce>()))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.Update(1, new Ecommerce()) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Delete_ShouldReturn200_WithDeletedItem(int id)
        {
            // Arrange
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
        public async Task Delete_ShouldReturn404_WithNotFoundItem()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync((Ecommerce)null);

            // Act
            var result = await _controller.Delete(1) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }
    }
}