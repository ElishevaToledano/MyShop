# MyShop - WEB API .NET 8

## Overview
MyShop is a RESTful Web API built with .NET 8, designed with **REST architecture best practices**.  
The project is structured in layers to ensure **scalability, maintainability, and clean code principles**.

---

## API Documentation
The API is documented using Swagger.  
You can access the API documentation at the following link:

https://localhost:44379/swagger/index.html

---

## Project Structure
The project is divided into multiple layers:

1. **Presentation Layer (Controllers)**
   - Handles HTTP requests and responses.
   - Example controllers: `UserController`, `ProductController`, `OrderController`, etc.

2. **Application Layer (Services, DTOs)**
   - Contains business logic and manages data transfer objects (DTOs).
   - Uses **AutoMapper** for DTO <-> Entity conversion.
   - Example DTOs: `RegisterUserDTO`, `OrderItemDTO`, `ProductDTO`, etc.

3. **Domain Layer (Entities, Interfaces)**
   - Represents core domain logic and entity definitions.
   - Example entities: `User`, `Order`, `Product`, `OrderItem`.

4. **Infrastructure Layer (Database, Repositories, Logging, Configuration)**
   - Handles persistence, logging, and configuration.
   - Uses **Entity Framework Core** with **Code First Migrations**.

---

## Why Layers?
- **Separation of Concerns** - Each layer has a distinct responsibility.
- **Scalability** - The architecture supports adding new features without affecting existing code.
- **Testability** - Layers make it easier to write **unit and integration tests**.

---

## Key Features

- **AutoMapper**: Used for mapping between DTOs and domain entities.
- **Dependency Injection (DI)**: Services and repositories are injected using .NET’s built-in DI to ensure loose coupling.
- **Asynchronous Processing**: Implemented using `async/await` for scalability and performance.
- **SQL Database with Code First Approach**: The database is managed using **Entity Framework Core Migrations**.
- **Configuration Management**: App settings are managed through config files.
- **Global Error Handling (Middleware)**:  
  - Errors are logged using a logging system.  
  - Critical errors are sent via **email notifications**.
- **Request Logging**: Every request is logged for analytics and rating purposes.

---

## Clean Code Principles
The project follows clean code principles to ensure maintainability:
- Meaningful variable and method names.
- Separation of concerns.
- Avoiding magic numbers and hardcoded values.
- Proper exception handling.

---

## Database Setup
To set up the database, use the following commands:
```sh
# Add migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

---

## Running the Project
Clone the repository:
```sh
git clone https://github.com/e0548547814/MyShop.git
```
Restore dependencies:
```sh
dotnet restore
```
Run the application:
```sh
dotnet run
```

---
 
## Contact Information
- **Name**: Elisheva Toledano
 **Phone**: 0548547814
- **Email**: 5860065t@gmail.com
