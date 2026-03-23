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
            // Arrange

            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreated()
        {
            // Arrange
            var product = new { name = "Test Product", price = 10.99m };

            // Act
            var response = await _client.PostAsJsonAsync("/products", product);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task ReadProducts_ReturnsOk()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/products");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var updatedProduct = new { name = "Updated Product", price = 12.99m };

            // Act
            var response = await _client.PutAsJsonAsync($"/products/{productId}", updatedProduct);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteProduct_ReturnsOk()
        {
            // Arrange
            var productId = 1;

            // Act
            var response = await _client.DeleteAsync($"/products/{productId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}