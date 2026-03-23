using Employee.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Employee.API.Tests.Integration
{
    public class EmployeeApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public EmployeeApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Health_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Get_Employees_ReturnsOk()
        {
            var response = await _client.GetAsync("/employees");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Post_Employee_ReturnsCreated()
        {
            var employee = new { name = "John Doe", department = "IT" };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(employee), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/employees", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Put_Employee_ReturnsOk()
        {
            var employee = new { id = 1, name = "Jane Doe", department = "HR" };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(employee), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/employees/1", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_Employee_ReturnsNoContent()
        {
            var response = await _client.DeleteAsync("/employees/1");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}