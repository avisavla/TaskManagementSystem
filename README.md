Task Management System

A full-stack Task Management System built using Angular and .NET 8 Web API.
The application allows users to authenticate, manage tasks, and track task status with filtering and pagination.

This project demonstrates JWT authentication, route protection, CRUD operations, REST API design, and error handling.

----------------------------------------------------------------------------------------------------------------------
Table of Contents

1. Project Overview
2. Tech Stack
3. Features
4. Task Workflow
5. Screenshots
6. Project Structure
7. API Endpoints
8. Setup Instructions
9. Authentication Flow
10. Error Handling
11. Future Improvements
12. Author
13. License

----------------------------------------------------------------------------------------------------------------------

1. Project Overview

The Task Management System allows authenticated users to manage tasks efficiently.

Users can:

* Log in using JWT authentication
* Create, update, and delete tasks
* View tasks with pagination
* Filter tasks by status
* Search tasks using text filtering

The application is built with a separated frontend and backend architecture using Angular and .NET.

----------------------------------------------------------------------------------------------------------------------

2. Tech Stack

Frontend

* Angular
* Angular Forms
* Angular Routing
* Angular HTTP Client
* Angular Material (Loading Spinner)

Backend

* .NET 8 Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger (API Documentation)

## Tools

* Git
* GitHub

----------------------------------------------------------------------------------------------------------------------

3. Features

Authentication

* User Login
* JWT Token Generation
* Secure API access
* Logout functionality
* Route protection using Angular Auth Guard
* HTTP Interceptor for attaching Authorization header

Task Management

* Create Task
* Edit Task
* Delete Task
* View Task List

Task Controls

* Pagination
* Page size selection
* Status filtering
* Text search

Validation and Error Handling

* Form validation
* Server validation error handling
* API error handling
* User friendly error messages

----------------------------------------------------------------------------------------------------------------------

4. Task Workflow

Tasks follow a simple lifecycle.

| Status     | Value |
| ---------- | ----- |
| Pending    | 0     |
| InProgress | 1     |
| Completed  | 2     |

Business Rules:

* Newly created tasks are automatically Pending
* Tasks cannot be deleted unless Completed
* API returns 409 Conflict if deletion is attempted on restricted tasks

----------------------------------------------------------------------------------------------------------------------

5. Screenshots

screenshots folder is inside task-management-ui repo 

Login Page=>(screenshots/login.png)

Task List=>(screenshots/taskList.png)

Create Task=>(screenshots/createTask.png)

Edit Task=>(screenshots/editTask.png)

----------------------------------------------------------------------------------------------------------------------

6. Project Structure

Frontend (Angular)

src/app
core
* guards
* interceptors
features
* auth
  * login
  * register
* tasks
  * task-list
  * create-task
  * edit-task
services
models

----------------------------------------------------------------------------------------------------------------------

Backend (.NET)

Controllers
* AuthController
* TasksController
Services
* JwtTokenService
Data
* ApplicationDbContext
Models
DTOs

----------------------------------------------------------------------------------------------------------------------

7. API Endpoints

Authentication

POST /api/auth/login
Login user and return JWT token.

POST /api/auth/register
Register a new user.

----------------------------------------------------------------------------------------------------------------------

Tasks

GET /api/tasks
Fetch tasks with pagination and filters.

Query parameters:
pageNo
pageSize
status
text

Example:
GET /api/tasks?pageNo=1&pageSize=5&status=Pending&text=test

----------------------------------------------------------------------------------------------------------------------

POST /api/tasks
Create a new task.

Request Body Example:

{
"name": "Sample Task",
"description": "Task description"
}

----------------------------------------------------------------------------------------------------------------------

PUT /api/tasks/{id}
Update task.

{
"name": "Updated Task",
"description": "Updated description",
"status": "Completed"
}

----------------------------------------------------------------------------------------------------------------------

DELETE /api/tasks/{id}

Delete task.
Business rule:

Only Completed tasks can be deleted.
If not, API returns 409 Conflict.

----------------------------------------------------------------------------------------------------------------------

8. Setup Instructions

Clone the Repositories

Clone both backend and frontend repositories:

git clone https://github.com/avisavla/TaskManagementSystem.git
git clone https://github.com/avisavla/task-management-ui.git

Navigate to the backend project:

cd TaskManagementSystem

----------------------------------------------------------------------------------------------------------------------

Backend Setup (.NET API)

Navigate to backend folder:cd backend

Update the connection string inside appsettings.json:

"ConnectionStrings": {
"DefaultConnection": "your_sql_connection_string"
}

Run database migration:dotnet ef database update

Run backend API:dotnet run

Backend will run on:https://localhost:44380

----------------------------------------------------------------------------------------------------------------------

Frontend Setup (Angular)

Navigate to the frontend repository:cd task-management-ui

Install dependencies:npm install

Run Angular application:ng serve

Application will run at:http://localhost:4200

----------------------------------------------------------------------------------------------------------------------

Application Access

Backend API: https://localhost:44380
Frontend UI: http://localhost:4200


----------------------------------------------------------------------------------------------------------------------

9. Authentication Flow

1. User logs in
2. API validates credentials
3. API returns JWT token
4. Token stored in browser
5. Angular HTTP interceptor attaches token to requests
6. Backend validates token for protected endpoints
7. Logout clears token

----------------------------------------------------------------------------------------------------------------------

10. Error Handling

The application handles common API errors.

| Status Code | Meaning                            |
| ----------- | ---------------------------------- |
| 400         | Validation error                   |
| 401         | Unauthorized                       |
| 404         | Resource not found                 |
| 409         | Conflict (business rule violation) |
| 500         | Server error                       |

----------------------------------------------------------------------------------------------------------------------

11. Future Improvements

Possible enhancements:

* Toast notifications
* Responsive design
* Unit tests
* Docker support
* Deployment to cloud

----------------------------------------------------------------------------------------------------------------------

12. Author - Avi Savla

Developed as a full-stack learning project using Angular and .NET 8.

----------------------------------------------------------------------------------------------------------------------

13. License

This project is open source and available under the MIT License.
