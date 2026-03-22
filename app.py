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
    customer_id: int
    order_date: str
    products: List[Product]

class Customer(BaseModel):
    id: int
    name: str
    email: str

# Sample in-memory data store
products = [
    Product(id=1, name="Product 1", price=10.99, description="This is product 1"),
    Product(id=2, name="Product 2", price=9.99, description="This is product 2"),
    Product(id=3, name="Product 3", price=12.99, description="This is product 3")
]

customers = [
    Customer(id=1, name="John Doe", email="john@example.com"),
    Customer(id=2, name="Jane Doe", email="jane@example.com")
]

orders = [
    Order(id=1, customer_id=1, order_date="2022-01-01", products=[products[0], products[1]]),
    Order(id=2, customer_id=2, order_date="2022-01-15", products=[products[2]])
]

# API Endpoints
@app.get("/products")
async def get_products():
    return products

@app.get("/products/{product_id}")
async def get_product(product_id: int):
    for product in products:
        if product.id == product_id:
            return product
    raise HTTPException(status_code=404, detail="Product not found")

@app.post("/products")
async def create_product(product: Product):
    products.append(product)
    return product

@app.put("/products/{product_id}")
async def update_product(product_id: int, product: Product):
    for existing_product in products:
        if existing_product.id == product_id:
            existing_product.name = product.name
            existing_product.price = product.price
            existing_product.description = product.description
            return existing_product
    raise HTTPException(status_code=404, detail="Product not found")

@app.delete("/products/{product_id}")
async def delete_product(product_id: int):
    for product in products:
        if product.id == product_id:
            products.remove(product)
            return {"message": "Product deleted successfully"}
    raise HTTPException(status_code=404, detail="Product not found")

@app.get("/customers")
async def get_customers():
    return customers

@app.get("/customers/{customer_id}")
async def get_customer(customer_id: int):
    for customer in customers:
        if customer.id == customer_id:
            return customer
    raise HTTPException(status_code=404, detail="Customer not found")

@app.post("/customers")
async def create_customer(customer: Customer):
    customers.append(customer)
    return customer

@app.put("/customers/{customer_id}")
async def update_customer(customer_id: int, customer: Customer):
    for existing_customer in customers:
        if existing_customer.id == customer_id:
            existing_customer.name = customer.name
            existing_customer.email = customer.email
            return existing_customer
    raise HTTPException(status_code=404, detail="Customer not found")

@app.delete("/customers/{customer_id}")
async def delete_customer(customer_id: int):
    for customer in customers:
        if customer.id == customer_id:
            customers.remove(customer)
            return {"message": "Customer deleted successfully"}
    raise HTTPException(status_code=404, detail="Customer not found")

@app.get("/orders")
async def get_orders():
    return orders

@app.get("/orders/{order_id}")
async def get_order(order_id: int):
    for order in orders:
        if order.id == order_id:
            return order
    raise HTTPException(status_code=404, detail="Order not found")

@app.post("/orders")
async def create_order(order: Order):
    orders.append(order)
    return order

@app.put("/orders/{order_id}")
async def update_order(order_id: int, order: Order):
    for existing_order in orders:
        if existing_order.id == order_id:
            existing_order.customer_id = order.customer_id
            existing_order.order_date = order.order_date
            existing_order.products = order.products
            return existing_order
    raise HTTPException(status_code=404, detail="Order not found")

@app.delete("/orders/{order_id}")
async def delete_order(order_id: int):
    for order in orders:
        if order.id == order_id:
            orders.remove(order)
            return {"message": "Order deleted successfully"}
    raise HTTPException(status_code=404, detail="Order not found")