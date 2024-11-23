
# .NET Mentorship Program - Nov 2024!




## Table of Contents
- [Overview](#overview)
- [Task 1](#task-1): ERP project using ASP.NET Core WebAPI
    - Task Accomplished
        - [Backend](#backend)
        - [Database](#database)
    - [Services](#services)
        - [Order management Services](#order-management-services-task-1)
    - [Necessary Nuget Packages / Libraries](#necessary-nuget-packages-/-libraries)
    - [Project Structure](#project-structure)
    - API Endpoints
    - API Documentations
        - Create New Order
        - Update Order
        - Delete order by id
        - List of all orders
        - Summary of ordered products
        - Retrive products below a threshold
        - Get top 3 customers
        - Find un-ordered products
        - Create Bulk Order
- Task 2: Creating Basic Authentication System (JWT Token Based)
    - Task Accomplished
        - Backend
        - Database
    - Services
        - Authentication Service
    - Necessary Nuget Packages / Libraries
    - Project Structure
    - API Endpoints
    - API Documentations
        - Register a New User
        - Login User
        - Access secure data (Token based Authorization)
- Technology Stack
- How to run
- Acknowledgement
- Limitations
- Conclusions



## Overview 🚀
The projects are created for the purpose of participating in .NET Mentorship Program - Nov 2024!
Here two projects were given:
-  ERP management system, &
- Basic Authentication System (JWT token based).
## Task 1: ERP project using ASP.NET Core WebAPI
### Task Accomplished
#### Backend
- [x] Write backend services using C# and ASP .NET Core 8.
- [x] Route management
- [x] Entity Framework Core (ORM) used
#### Database
- [x] MS SQL database integration
### Services
#### Order management services
- Create, Read, Update and Delete orders
- List all orders
- Summaray of ordered products, List un-ordered prodcts
- Retrive Products below threshold, Retrive Top 3 customers
- Creating bulk orders
### Necessary Nuget Packages / Libraries
Go to 'Manage NuGet Packages' or Open Package manager console in Visual Studio, then install below packages:
```bash
1.  Install-Package Microsoft.EntityFramework.Core
```
```bash
2.  Install-Package Microsoft.EntityFramework.Core.Tools
```
```bash
3.  Install-Package Microsoft.EntityFramework.Core.SQLServer
```
### Project Structures
- `ERP Project` for creating the ERP management system.
- `Controllers` for creating api Controllers.
- `Controllers/OrdersController` for managing api endpoints of order.
- `Controllers/ProductsController` for managing api endpoints of product.
- `Models` for managing database.
- `Program.cs` for Dependency Injection, Configuration of different services.
### API Endpoints
#### Orders Service (`localhost:7253/api/Orders`)
- `GET /` - List all orders
- `POST /` - Create new order
- `GET /{id}` - Get order by ID
- `PUT /{id}` - Update an order
- `DELETE /{id}` - Delete an order 
- `GET /Products/Summary` - Get a summary of total quantity ordered and total revenue for each product
- `GET /Customers` - Get the top 3 customers by total quantity ordered
- `POST /BulkOrderCreation` - A transactional operation for bulk order creation
#### Products Service (`localhost:7253/api/Products`)
- `GET /LowStock` - Retrieve all products with a stock quantity below a specified threshold
- `GET /UnOrdered` - Retrive the products that have not been ordered at all
### API Documentations
Here is the list of all api endpoints using curl command and Swagger UI.
#### Orders Services
##### List all orders
For listing all orders you may use below curl command:
```
curl -X 'GET' \
  'https://localhost:7253/api/Orders' \
  -H 'accept: */*'
  ```
The response will be `200 OK` with following response body:
```
[
  {
    "id": 9,
    "customerName": "John Doe",
    "quantity": 20,
    "orderDate": "2024-01-15T20:00:00+06:00",
    "productName": "Widget A",
    "unitPrice": 50
  },
  {
    "id": 10,
    "customerName": "Jane Smith",
    "quantity": 10,
    "orderDate": "2024-02-10T16:30:00+06:00",
    "productName": "Gadget B",
    "unitPrice": 75
  },
  {
    "id": 11,
    "customerName": "Sam Wilson",
    "quantity": 15,
    "orderDate": "2024-03-05T15:15:00+06:00",
    "productName": "Widget A",
    "unitPrice": 50
  },
  {
    "id": 12,
    "customerName": "Alex Brown",
    "quantity": 25,
    "orderDate": "2024-11-17T23:30:38.0489445+06:00",
    "productName": "Tool C",
    "unitPrice": 100
  },
  {
    "id": 14,
    "customerName": "Sourav Kundu",
    "quantity": 100,
    "orderDate": "2024-11-17T23:46:56.1394414+06:00",
    "productName": "Gadget B",
    "unitPrice": 75
  },
  {
    "id": 16,
    "customerName": "Sourav Sk",
    "quantity": 20,
    "orderDate": "2024-11-17T23:53:28.4312624+06:00",
    "productName": "Widget A",
    "unitPrice": 50
  },
  {
    "id": 17,
    "customerName": "SouravSk",
    "quantity": 50,
    "orderDate": "2024-11-18T23:14:18.6225064+06:00",
    "productName": "Extra Product",
    "unitPrice": 150
  },
  {
    "id": 18,
    "customerName": "Sk",
    "quantity": 250,
    "orderDate": "2024-11-18T23:14:55.3633488+06:00",
    "productName": "ETC",
    "unitPrice": 10
  },
  {
    "id": 19,
    "customerName": "ssk",
    "quantity": 50,
    "orderDate": "2024-11-18T23:20:47.5811759+06:00",
    "productName": "ETC",
    "unitPrice": 10
  },
  {
    "id": 20,
    "customerName": "ssk1",
    "quantity": 100,
    "orderDate": "2024-11-18T23:20:47.5834875+06:00",
    "productName": "ETC",
    "unitPrice": 10
  }
]
```
##### Create New Order
Below curl can be used for creating new order with body:
```
curl -X 'POST' \
  'https://localhost:7253/api/Orders' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "productId": 19,
  "customerName": "SK",
  "quantity": 20
}'
```
Respone will be `201 OK` with following
~~~
{
  "id": 21,
  "customerName": "SK",
  "quantity": 20,
  "orderDate": "2024-11-22T21:07:34.4829409Z",
  "productId": 19
}
~~~
##### Update Order
Below curl can be used for updating an order by id with body
```
curl -X 'PUT' \
  'https://localhost:7253/api/Orders/21' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "productId": 18,
  "customerName": "SK",
  "quantity": 50
}'
```
Respone will be `201 OK` with following
~~~
{
  "id": 21,
  "customerName": "SK",
  "quantity": 50,
  "orderDate": "2024-11-22T21:07:34.4829409",
  "productId": 19
}
~~~
##### Delete order by id
Below curl can be used for deleting an order by id, this will take id by form query, success status will be `200 OK` and failed request output will be `404 not found`.

##### Get summary 
Below curl can be used for getting summary with product detail:
~~~
curl -X 'GET' \
  'https://localhost:7253/api/Orders/Products/Summary' \
  -H 'accept: */*'
~~~
Respone will be `201 OK` with following
~~~
[
  {
    "productName": "Widget A",
    "totalQuantityOrdered": 55,
    "totalRevenue": 2750
  },
  {
    "productName": "Gadget B",
    "totalQuantityOrdered": 110,
    "totalRevenue": 8250
  },
  {
    "productName": "Tool C",
    "totalQuantityOrdered": 25,
    "totalRevenue": 2500
  },
  {
    "productName": "Extra Product",
    "totalQuantityOrdered": 50,
    "totalRevenue": 7500
  },
  {
    "productName": "ETC",
    "totalQuantityOrdered": 400,
    "totalRevenue": 4000
  }
]
~~~
#####  Get the top 3 customers
Below curl can be used for getting the top 3 customers
~~~
curl -X 'GET' \
  'https://localhost:7253/api/Orders/Customers' \
  -H 'accept: */*'
~~~
Respone will be `201 OK` with following
~~~
[
  {
    "customerName": "Sk",
    "totalQuantity": 250
  },
  {
    "customerName": "Sourav Kundu",
    "totalQuantity": 100
  },
  {
    "customerName": "ssk1",
    "totalQuantity": 100
  }
]
~~~
##### Bulk order creation
Below curl can be used for bulk order creation
~~~
curl -X 'POST' \
  'https://localhost:7253/api/Orders/BulkOrderCreation' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '[
  {
    "productId": 10,
    "customerName": "SK2",
    "quantity": 21
  },
  {
    "productId": 19,
    "customerName": "SK1",
    "quantity": 15
  }
]
~~~
For error response will be `400` and for success will show `Orders created successfully`.
#### Products Service
##### Retrive products below a threshold
Below curl can be used for Retrive products below a threshold
~~~
curl -X 'GET' \
  'https://localhost:7253/api/Products/LowStock?threshold=100' \
  -H 'accept: */*'
~~~
It will take value by form query and show response `200 OK` with body
```
[
  {
    "productName": "Gadget B",
    "unitPrices": 75,
    "stock": 50
  }
]
```
##### Retrived Un-Ordered products
Below curl can be used for Retrived Un-Ordered products
~~~
curl -X 'GET' \
  'https://localhost:7253/api/Products/UnOrdered' \
  -H 'accept: */*'
~~~
For failure it will show `404 not found` and for success it will show `200 OK` and provide response body.
## Task 2: Creating Basic Authentication System (JWT Token Based)
### Task Accomplished
#### Backend
- [x] Write backend services using C# and ASP .NET Core 8.
- [x] Route management
- [x] Entity Framework Core (ORM) used
#### Database
- [x] MS SQL database integration
### Services
#### Authentication Service
- User registration and authentication
- User login with username and password
- JWT token generation and validation
- Secure password hashing
### Necessary Nuget Packages / Libraries
Go to 'Manage NuGet Packages' or Open Package manager console in Visual Studio, then install below packages:
```bash
1.  Install-Package Microsoft.EntityFramework.Core
```
```bash
2.  Install-Package Microsoft.EntityFramework.Core.Tools
```
```bash
3.  Install-Package Microsoft.EntityFramework.Core.SQLServer
```
```bash
4.  Install-Package Microsoft.Asp.NetCore.Authentication.Jwt Bearer
```
### Project Structures
- `Basic_Auth_System` for creating the Basic Authentication System.
- `Controllers` for creating api Controllers.
- `Controllers/AuthController` for managing api endpoints 
- `Models` for managing database.
- `Program.cs` for Dependency Injection, Configuration of different services.
### API Endpoints
#### Authentication Service (localhost:7130/api/v1/Auth)
- `POST /register` - Register new user
- `POST /login` - User login and Generate JWT Token
- `GET /secure-data` - Access Authorized data
### API Documentations
Here is the list of all api endpoints using curl command and Swagger UI.
#### Authentication Service
##### Register user
For register a new user, you can use the following curl command, it takes data from query.
~~~
curl -X 'POST' \
  'https://localhost:7130/api/v1/Auth/register' \
  -H 'accept: */*' \
  -d ''
~~~
For the registration, I have used the username and password as the required fields. and the password must contain atleast one uppercase, one lowercase, one special character and one number that validates password, and the username must be unique.
The response will be status 201 Created with the no response body.
##### Login User
For login a user, you can use the following curl command, this also takes data from query.
~~~
curl -X 'POST' \
  'https://localhost:7130/api/v1/Auth/login' \
  -H 'accept: */*' \
~~~
The response will be status `200 OK` with the following response body:
~~~
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic291cmF2MSIsImV4cCI6MTczMjMxNjg3MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEzMCIsImF1ZCI6Im15LWxvY2FsLWFwaSJ9.R4QoeZ9Tox_FKoVyA8_EQUnALX4b3j-tZN5IQvbo-lY",
  "user": {
    "id": 2,
    "username": "sourav1",
    "passwordHash": "Jtjz08xCu6nPTfJAD3bvXe0Srgd2TKzuKnNoRNR4Tuc=",
    "failedLoginAttempts": 0,
    "lockoutEnd": null
  }
}
~~~
When the credentials are not correct, the response will be status `code 401` with message Invalid credentials and if locked out by providing error credentials more than 3 times shows `error code 500`
##### Access Authorized data
For this we have to use postman, in postman we have to copy the link and select auth as authorize and provide the token then send.
If success shows `200 OK` with success message and for errors shows code `401 unauthorized`.

## Technology Stack

**Backend:** C#

**Framework:** ASP .NET Core, WebAPI 8

**Database:** MS SQL Server

**Authentication:** JWT Token

**ORM:** Entity Framework Core

## How to Run
    1. Clone the repository using (https://github.com/SouravKunduSK/Dot_NET_Mentorship_Program_Nov_24_Tasks.git)
    2. Download necessary packages
    3. Connect to Database
    4. Select the desired project as default project and Run.

## Acknowledgements
 - As I use short lived jwt token for authentication, i do not implement the logout functionality in the auth service.
 - I tried to build web application for Basic_Auth_System but can not be succeeded because of using JWT token.

## Limitations
- [] Not used email for registe/login.
- [] Not implemented refresh token, as I'm newbee for webAPI.
- [] No Unit Testing applied.

## Conclusions
This is my first time I have worked in WebApi. I faced some challenges, but I learned a lot from them. Thank you for your time and consideration.

