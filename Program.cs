using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", () => 
{
    var products = new[]
    {
        new { Id = 1, Name = "Product 1", Price = 10.99m },
        new { Id = 2, Name = "Product 2", Price = 5.99m },
        new { Id = 3, Name = "Product 3", Price = 7.99m },
    };
    return Results.Ok(products);
});

app.MapGet("/products/{id}", (int id) => 
{
    var product = new { Id = id, Name = "Product " + id, Price = 10.99m };
    return Results.Ok(product);
});

app.MapPost("/products", (Product product) => 
{
    // Save product to database
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", (int id, Product product) => 
{
    // Update product in database
    return Results.NoContent();
});

app.MapDelete("/products/{id}", (int id) => 
{
    // Delete product from database
    return Results.NoContent();
});

app.Run();

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}