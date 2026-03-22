using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

var products = new[]
{
    new { Id = 1, Name = "Product 1", Price = 10.99m },
    new { Id = 2, Name = "Product 2", Price = 9.99m },
    new { Id = 3, Name = "Product 3", Price = 12.99m },
};

app.MapGet("/products", () => Results.Ok(products));
app.MapGet("/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product != null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/products", (Product product) =>
{
    products = products.Concat(new[] { product }).ToArray();
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", (int id, Product product) =>
{
    var index = Array.IndexOf(products, products.FirstOrDefault(p => p.Id == id));
    if (index != -1)
    {
        products[index] = product;
        return Results.Ok(product);
    }
    return Results.NotFound();
});

app.MapDelete("/products/{id}", (int id) =>
{
    products = products.Where(p => p.Id != id).ToArray();
    return Results.NoContent();
});

app.Run();

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}