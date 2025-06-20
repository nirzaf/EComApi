# ECom API

A backend API for an e-commerce application, providing full CRUD operations for managing customers, products, categories, and orders.

## Table of Contents

- [Project Overview](#project-overview)
- [Data Model](#data-model)
- [API Endpoints](#api-endpoints)
- [Setup Instructions](#setup-instructions)
- [Testing](#testing)
- [Contributing](#contributing)

## Project Overview

This project implements a minimalistic backend web application for an online shop, providing RESTful APIs for managing:
- Customers
- Shop Item Categories
- Shop Items (Products)
- Orders

## Data Model

### Customer
- ID (integer)
- Name (string)
- Surname (string)
- Email (string)

### ShopItemCategory
- ID (integer)
- Title (string)
- Description (string)

### ShopItem
- ID (integer)
- Title (string)
- Description (string)
- Price (float)
- Category (list of ShopItemCategory)

### OrderItem
- ID (integer)
- ShopItem (ShopItem)
- Quantity (integer)

### Order
- ID (integer)
- Customer (Customer)
- Items (list of OrderItem)

## API Endpoints

Full CRUD APIs are implemented for all entities:
- GET, POST, PUT, DELETE for Customers
- GET, POST, PUT, DELETE for ShopItemCategories
- GET, POST, PUT, DELETE for ShopItems
- GET, POST, PUT, DELETE for Orders

## Setup Instructions

1. Clone the repository
2. Install dependencies
3. Initialize database with test data
4. Run the application

## Testing

This project uses xUnit for testing. Tests are organized by entity and include comprehensive coverage for all API endpoints.

To run the tests:

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~EComApi.Tests.<TestClassName>"

# Run specific test method
dotnet test --filter "FullyQualifiedName~EComApi.Tests.<TestClassName>.<TestMethodName>"
```

The test project includes:
- Integration tests for all CRUD operations
- Test data initialization
- Mocked dependencies where appropriate
- Asserts for HTTP status codes and response content

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

