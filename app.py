from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List

app = FastAPI()

class Product(BaseModel):
    id: int
    name: str
    price: float
    description: str

class Order(BaseModel):
    id: int
    product_id: int
    quantity: int

class Customer(BaseModel):
    id: int
    name: str
    email: str

products = [
    {"id": 1, "name": "Product 1", "price": 10.99, "description": "This is product 1"},
    {"id": 2, "name": "Product 2", "price": 9.99, "description": "This is product 2"},
    {"id": 3, "name": "Product 3", "price": 12.99, "description": "This is product 3"}
]

orders = []
customers = []

@app.get("/products/")
async def read_products():
    return products

@app.get("/products/{product_id}")
async def read_product(product_id: int):
    for product in products:
        if product["id"] == product_id:
            return product
    raise HTTPException(status_code=404, detail="Product not found")

@app.post("/products/")
async def create_product(product: Product):
    products.append(product.dict())
    return product

@app.put("/products/{product_id}")
async def update_product(product_id: int, product: Product):
    for i, p in enumerate(products):
        if p["id"] == product_id:
            products[i] = product.dict()
            return product
    raise HTTPException(status_code=404, detail="Product not found")

@app.delete("/products/{product_id}")
async def delete_product(product_id: int):
    for i, p in enumerate(products):
        if p["id"] == product_id:
            del products[i]
            return {"message": "Product deleted successfully"}
    raise HTTPException(status_code=404, detail="Product not found")

@app.post("/orders/")
async def create_order(order: Order):
    orders.append(order.dict())
    return order

@app.get("/orders/")
async def read_orders():
    return orders

@app.get("/orders/{order_id}")
async def read_order(order_id: int):
    for order in orders:
        if order["id"] == order_id:
            return order
    raise HTTPException(status_code=404, detail="Order not found")

@app.post("/customers/")
async def create_customer(customer: Customer):
    customers.append(customer.dict())
    return customer

@app.get("/customers/")
async def read_customers():
    return customers

@app.get("/customers/{customer_id}")
async def read_customer(customer_id: int):
    for customer in customers:
        if customer["id"] == customer_id:
            return customer
    raise HTTPException(status_code=404, detail="Customer not found")