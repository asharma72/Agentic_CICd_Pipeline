using Ecommerce.API.Controllers;
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
            var items = new List<Ecommerce> { new Ecommerce { Id = 1 }, new Ecommerce { Id = 2 } };
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(items);
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
        public async Task GetById_ShouldReturn404_WithInvalidId()
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
        public async Task Create_ShouldReturn201_WithItem()
        {
            // Arrange
            var item = new Ecommerce { Id = 1 };
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
            var item = new Ecommerce { Id = 0 };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Ecommerce>()))
                .Throws(new Exception());

            // Act
            var result = await _controller.Create(item) as BadRequestObjectResult;

            // Assert
            result?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Update_ShouldReturn200_WithItem()
        {
            // Arrange
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Update(item) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Update_ShouldReturn404_WithInvalidId()
        {
            // Arrange
            var item = new Ecommerce { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Ecommerce>()))
                .Throws(new Exception());

            // Act
            var result = await _controller.Update(item) as NotFoundResult;

            // Assert
            result?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Delete_ShouldReturn200()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1) as OkResult;

            // Assert
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_ShouldReturn404_WithInvalidId()
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