Task Management System – Backend API

.NET 8 Web API backend for the Task Management System.

This API provides authentication and task management endpoints used by the Angular frontend.

It implements JWT authentication, CRUD operations, pagination, filtering, and validation handling.

----------------------------------------------------------------------------------------------------------------------------------------------

Table of Contents

1. Project Overview
2. Tech Stack
3. Features
4. Task Workflow
5. Project Structure
6. API Endpoints
7. Setup Instructions
8. Authentication Flow
9. Error Handling
10. Frontend Repository
11. Author
12. License

----------------------------------------------------------------------------------------------------------------------------------------------

1. Project Overview

The backend API is built using ASP.NET Core (.NET 8).

It handles:

* User authentication
* Task management
* Database access
* Business rule validation
* Secure API endpoints using JWT

The API communicates with the Angular frontend through REST endpoints.

----------------------------------------------------------------------------------------------------------------------------------------------

2. Tech Stack

Backend Framework

* .NET 8 Web API

Database

* SQL Server
* Entity Framework Core

Authentication

* JWT Authentication

Documentation

* Swagger 

Tools

* Git
* GitHub

----------------------------------------------------------------------------------------------------------------------------------------------

3. Features

Authentication

* User Login
* User Registration
* JWT Token generation
* Secure API endpoints

Task Management

* Create Task
* Update Task
* Delete Task
* Get task list

Task Controls

* Pagination
* Status filtering
* Text search

Validation and Error Handling

* Model validation
* Business rule validation
* Proper HTTP status codes

----------------------------------------------------------------------------------------------------------------------------------------------

4. Task Workflow

Tasks follow a lifecycle.

| Status     | Value |
| ---------- | ----- |
| Pending    | 0     |
| InProgress | 1     |
| Completed  | 2     |

Business Rules:

* Newly created tasks are automatically Pending
* Tasks cannot be deleted unless Completed
* API returns 409 Conflict if deletion rule is violated

----------------------------------------------------------------------------------------------------------------------------------------------

5. Project Structure

Controllers

* AuthController
* TasksController

Services

* JwtTokenService

Data

* ApplicationDbContext

Models

DTOs

----------------------------------------------------------------------------------------------------------------------------------------------

6. API Endpoints

Authentication

POST /api/auth/login
Login user and return JWT token.

POST /api/auth/register
Register a new user.

----------------------------------------------------------------------------------------------------------------------------------------------

Tasks

GET /api/tasks

Fetch tasks with pagination and filters.

Query parameters:

pageNo
pageSize
status
text

Example

GET /api/tasks?pageNo=1&pageSize=5&status=Pending&text=test

----------------------------------------------------------------------------------------------------------------------------------------------

POST /api/tasks

Create a new task.

Example request body

{
"name": "Sample Task",
"description": "Task description"
}

----------------------------------------------------------------------------------------------------------------------------------------------

PUT /api/tasks/{id}

Update task

{
"name": "Updated Task",
"description": "Updated description",
"status": "Completed"
}

----------------------------------------------------------------------------------------------------------------------------------------------

DELETE /api/tasks/{id}

Delete task.

Rule:

Only Completed tasks can be deleted.
Otherwise API returns 409 Conflict.

----------------------------------------------------------------------------------------------------------------------------------------------

7. Setup Instructions

Clone the backend repository

git clone https://github.com/avisavla/TaskManagementSystem.git

Navigate to project

cd TaskManagementSystem

----------------------------------------------------------------------------------------------------------------------------------------------

Configure Database

Update connection string in appsettings.json

"ConnectionStrings": {
"DefaultConnection": "your_sql_connection_string"
}

----------------------------------------------------------------------------------------------------------------------------------------------

Run Database Migration

dotnet ef database update

----------------------------------------------------------------------------------------------------------------------------------------------

Run API

dotnet run

Backend runs on

https://localhost:44380

----------------------------------------------------------------------------------------------------------------------------------------------

8. Authentication Flow

1. User sends login request
2. API validates credentials
3. API generates JWT token
4. Client stores token
5. Client sends token in Authorization header
6. API validates token before allowing access

----------------------------------------------------------------------------------------------------------------------------------------------

9. Error Handling

| Status Code | Meaning                            |
| ----------- | ---------------------------------- |
| 400         | Validation error                   |
| 401         | Unauthorized                       |
| 404         | Resource not found                 |
| 409         | Conflict (business rule violation) |
| 500         | Server error                       |

----------------------------------------------------------------------------------------------------------------------------------------------

10. Frontend Repository

Angular UI repository:

https://github.com/avisavla/task-management-ui

----------------------------------------------------------------------------------------------------------------------------------------------

11. Author

Avi Savla

Backend built using ASP.NET Core (.NET 8) as part of a full-stack learning project.

----------------------------------------------------------------------------------------------------------------------------------------------

12. License

This project is open source and available under the MIT License.
