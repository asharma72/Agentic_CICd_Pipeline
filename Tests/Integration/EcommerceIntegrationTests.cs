using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.API.Tests.Integration
{
    public class EcommerceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public EcommerceApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Health_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_Product_ReturnsCreated()
        {
            var product = new { Name = "Test Product", Price = 10.99m };
            var response = await _client.PostAsJsonAsync("/products", product);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Products_ReturnsOk()
        {
            var response = await _client.GetAsync("/products");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_ProductById_ReturnsOk()
        {
            var response = await _client.GetAsync("/products/1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_Product_ReturnsOk()
        {
            var product = new { Id = 1, Name = "Updated Test Product", Price = 12.99m };
            var response = await _client.PutAsJsonAsync("/products/1", product);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_Product_ReturnsOk()
        {
            var response = await _client.DeleteAsync("/products/1");
            response.EnsureSuccessStatusCode();
        }
    }
}