Project Task Management System

A clean and modular Task Management API built using ASP.NET Core 8, Entity Framework Core, and Clean Architecture principles.
This API allows managing projects and tasks, supporting CRUD operations, filtering, and authentication — designed to demonstrate production-level code quality and maintainability.

🧱 Architecture Overview

The solution follows the Clean Architecture pattern, ensuring separation of concerns and testability.

ProjectTaskManagementSystem
│
├── ProjectManagementSystem                → API Layer (Controllers, Middlewares, Configurations)
├── ProjectManagementSystem.Application     → Application Layer (Services, DTOs, Interfaces, CQRS)
├── ProjectManagementSystem.Domain          → Domain Layer (Entities, Enums, Core Logic)
├── ProjectManagementSystem.Infrastructure  → Infrastructure Layer (EF Core, Repository Implementations)
└── TaskManagementSystem.Test               → Test Layer (xUnit/NUnit Unit Tests)

🚀 Features

✅ CRUD for Projects and Tasks

🔒 JWT Authentication for secure endpoints

⚙️ Entity Framework Core for data access

🧩 CQRS with MediatR for scalable command/query separation

🧠 DTOs and Validation (FluentValidation/DataAnnotations)

💾 Caching for GET endpoints

🪶 Swagger/OpenAPI documentation

🧵 Async/await for non-blocking operations

🧰 Centralized Exception Handling and Logging Middleware

🧪 Unit Tests with xUnit/NUnit

🧩 Dependency Injection for all services and repositories

🧩 Entities
Project
Field	Type	Description
Id	int	Primary Key
Name	string	Required, max 200 chars
Description	string	Optional
StartDate	DateTime	Project start date
EndDate	DateTime?	Project end date
Status	enum (Planned, Active, Completed, OnHold)	Project status
TaskItem
Field	Type	Description
Id	int	Primary Key
Title	string	Required
Description	string	Optional
AssignedTo	string	Optional
DueDate	DateTime?	Optional
Status	enum (Pending, InProgress, Done)	Task status
ProjectId	int	Foreign Key (Project)
🌐 API Endpoints
Method	Route	Description
GET	/api/projects	List all projects (filter by status, pagination)
GET	/api/projects/{id}	Get project details (with tasks)
POST	/api/projects	Create a new project
PUT	/api/projects/{id}	Update project details
DELETE	/api/projects/{id}	Delete project and its tasks
GET	/api/tasks	List all tasks (filter by project, status, assigned user)
POST	/api/tasks	Create a new task
PUT	/api/tasks/{id}	Update task details
DELETE	/api/tasks/{id}	Delete a task
⚙️ Tech Stack

ASP.NET Core 8

Entity Framework Core (InMemory / SQLite / SQL Server LocalDB)

C# 12

AutoMapper / Mapster

JWT Authentication

Swagger/OpenAPI

xUnit / NUnit

MediatR (CQRS Pattern)

🧠 Design Decisions

Implemented Clean Architecture for maintainability and scalability.

Used CQRS to separate command and query logic.

Introduced DTOs and validation layers to ensure data consistency.

Added global exception handling for uniform API responses.

Integrated JWT Authentication for security and authorization.

Configured Swagger for easy API testing and documentation.

Added caching to improve read performance on GET endpoints.

🧪 Testing

Unit tests for core services and business logic.

Mocks for repositories to ensure isolation.

Test project: TaskManagementSystem.Test.

Run tests via:

dotnet test

🧰 How to Run the Project

Clone the repository:

git clone https://github.com/yourusername/ProjectTaskManagementSystem.git


Navigate to the API project:

cd ProjectTaskManagementSystem/ProjectManagementSystem


Restore dependencies:

dotnet restore


Run the application:

dotnet run


Access Swagger UI:

https://localhost:5001/swagger

🧭 Future Improvements

Add role-based authorization (Admin/Manager/User)

Implement advanced filtering & sorting

Integrate with Redis for distributed caching

Containerize using Docker

Add integration tests

👨‍💼 Team Lead Reflection

As a team lead, my approach emphasizes mentorship, clean code, and collaboration.
I would guide junior developers by explaining why architectural choices are made — not just how.
Code reviews would focus on readability, test coverage, and adherence to SOLID principles.
We’d maintain a shared coding standard document outlining naming conventions (PascalCase for classes, camelCase for variables), clear folder structures, and consistent commit messages.
Regular peer reviews and knowledge-sharing sessions would ensure everyone grows technically and understands the bigger picture.
My focus is always on building maintainable, testable, and scalable software — together, as a unified engineering team.
