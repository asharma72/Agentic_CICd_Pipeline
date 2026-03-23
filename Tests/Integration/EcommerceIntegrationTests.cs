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
        public async Task Get_Health_ReturnsOkResponse()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/health");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_Product_ReturnsCreatedResponse()
        {
            // Arrange
            var newProduct = new { Name = "Test Product", Price = 10.99m };
            var request = new HttpRequestMessage(HttpMethod.Post, "/products")
            {
                Content = new System.Text.Json.JsonContent(newProduct)
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Get_Products_ReturnsOkResponse()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/products");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_ProductById_ReturnsOkResponse()
        {
            // Arrange
            var newProduct = new { Name = "Test Product", Price = 10.99m };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, "/products")
            {
                Content = new System.Text.Json.JsonContent(newProduct)
            };
            var createResponse = await _client.SendAsync(createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createdProductId = await createResponse.Content.ReadAsAsync<int>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/products/{createdProductId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_Product_ReturnsOkResponse()
        {
            // Arrange
            var newProduct = new { Name = "Test Product", Price = 10.99m };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, "/products")
            {
                Content = new System.Text.Json.JsonContent(newProduct)
            };
            var createResponse = await _client.SendAsync(createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createdProductId = await createResponse.Content.ReadAsAsync<int>();

            var updateProduct = new { Name = "Updated Product", Price = 12.99m };
            var request = new HttpRequestMessage(HttpMethod.Put, $"/products/{createdProductId}")
            {
                Content = new System.Text.Json.JsonContent(updateProduct)
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_Product_ReturnsNoContentResponse()
        {
            // Arrange
            var newProduct = new { Name = "Test Product", Price = 10.99m };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, "/products")
            {
                Content = new System.Text.Json.JsonContent(newProduct)
            };
            var createResponse = await _client.SendAsync(createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createdProductId = await createResponse.Content.ReadAsAsync<int>();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/products/{createdProductId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}