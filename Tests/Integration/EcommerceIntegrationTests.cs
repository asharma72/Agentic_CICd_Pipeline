using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
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
        public async Task GetHealth_Healthy_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/health");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkWithProducts()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/products");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.NotNull(products);
        }

        [Fact]
        public async Task GetProductById_ReturnsOkWithProduct()
        {
            // Arrange
            var productId = 1;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/products/{productId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var product = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(product);
            Assert.Equal(productId, product.Id);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedWithProduct()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 10.99m };
            var request = new HttpRequestMessage(HttpMethod.Post, "/products")
            {
                Content = JsonContent.Create(product)
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(createdProduct);
            Assert.Equal(product.Name, createdProduct.Name);
            Assert.Equal(product.Price, createdProduct.Price);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOkWithUpdatedProduct()
        {
            // Arrange
            var productId = 1;
            var updatedProduct = new Product { Id = productId, Name = "Updated Test Product", Price = 12.99m };
            var request = new HttpRequestMessage(HttpMethod.Put, $"/products/{productId}")
            {
                Content = JsonContent.Create(updatedProduct)
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var product = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(product);
            Assert.Equal(updatedProduct.Id, product.Id);
            Assert.Equal(updatedProduct.Name, product.Name);
            Assert.Equal(updatedProduct.Price, product.Price);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            // Arrange
            var productId = 1;
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/products/{productId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}