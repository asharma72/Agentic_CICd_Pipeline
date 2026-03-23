using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public async Task HealthEndpoint_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/health");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreatedResponse()
        {
            var product = new { Name = "Test Product", Price = 10.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/products", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResponseWithProducts()
        {
            var response = await _client.GetAsync("/products");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadAsAsync<Product[]>();
            Assert.NotNull(products);
        }

        [Fact]
        public async Task PutProduct_ReturnsOkResponse()
        {
            var product = new { Id = 1, Name = "Updated Test Product", Price = 10.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/products/1", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContentResponse()
        {
            var response = await _client.DeleteAsync("/products/1");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}