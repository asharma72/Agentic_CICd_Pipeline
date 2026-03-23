using Employee.API.Controllers;
using Employee.API.Models;
using Employee.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Employee.API.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockService;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockService = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturn200_WithItems()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Employee>());
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAll_ShouldReturn200_WithNoItems()
        {
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Employee>());
            var result = await _controller.GetAll() as OkObjectResult;
            result?.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ShouldReturn200_WithItem(int id)
        {
            var employee = new Employee { Id = id };
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(employee);
            var result = await _controller.GetById(id) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeOfType<Employee>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ShouldReturn404_WithNoItem(int id)
        {
            _mockService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync((Employee)null);
            var result = await _controller.GetById(id);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_ShouldReturn201_WithItem()
        {
            var employee = new Employee { Name = "Test" };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Employee>()))
                .ReturnsAsync(employee);
            var result = await _controller.Create(employee) as CreatedAtActionResult;
            result?.StatusCode.Should().Be(201);
            result?.Value.Should().BeOfType<Employee>();
        }

        [Fact]
        public async Task Create_ShouldReturn400_WithInvalidItem()
        {
            var employee = new Employee { Name = null };
            _controller.ModelState.AddModelError("Name", "Name is required");
            var result = await _controller.Create(employee);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Update_ShouldReturn200_WithItem(int id)
        {
            var employee = new Employee { Id = id, Name = "Test" };
            _mockService.Setup(s => s.UpdateAsync(id, It.IsAny<Employee>()))
                .ReturnsAsync(employee);
            var result = await _controller.Update(id, employee) as OkObjectResult;
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeOfType<Employee>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Update_ShouldReturn404_WithNoItem(int id)
        {
            var employee = new Employee { Id = id, Name = "Test" };
            _mockService.Setup(s => s.UpdateAsync(id, It.IsAny<Employee>()))
                .ReturnsAsync((Employee)null);
            var result = await _controller.Update(id, employee);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Update_ShouldReturn400_WithInvalidItem(int id)
        {
            var employee = new Employee { Id = id, Name = null };
            _controller.ModelState.AddModelError("Name", "Name is required");
            var result = await _controller.Update(id, employee);
            result.Should().BeOfType<BadRequestObjectResult>();
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
        [InlineData(1)]
        [InlineData(2)]
        public async Task Delete_ShouldReturn404_WithNoItem(int id)
        {
            _mockService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(false);
            var result = await _controller.Delete(id);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}