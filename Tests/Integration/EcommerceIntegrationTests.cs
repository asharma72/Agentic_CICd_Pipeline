using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.API.Tests.Integration
{
    public class EcommerceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EcommerceApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostProduct_CreatesProduct()
        {
            var product = new { Name = "Test Product", Price = 10.99m };
            var response = await _client.PostAsJsonAsync("/products", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ReturnsProducts()
        {
            var response = await _client.GetAsync("/products");
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PutProduct_UpdatesProduct()
        {
            var product = new { Id = 1, Name = "Updated Product", Price = 10.99m };
            var response = await _client.PutAsJsonAsync("/products/1", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_RemovesProduct()
        {
            var response = await _client.DeleteAsync("/products/1");
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}