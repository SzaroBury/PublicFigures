# **RespectCounter (Work in Progress)**

**RespectCounter** is a .NET + React application that allows users to express their opinion on various public figures, their quotes and actions.

---

## **Features**
- Users can browse a catalog of public figures, activities, quotes and comments.
- Users can filter quotes, activities and public figures using tags.
- Users can sign up and log in using their accounts.
- Logged-in users can:
    - Propose new public figures.
    - Add quotes and activities and link them to existing public figures.
    - Comment on and react to public figures, activities, and other comments.
    <!-- - Report activities, quotes and comments of others. -->
- Admins can:
    - Verify public figures, quotes and activities added by users.
    <!-- - Hide comments and activities. -->

---

## **Technology Stack**
- .NET 8
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core
- MediatR 12.4.1
- MS SQL Server
- React 18.2.0
- React-Bootstrap 2.8.0

---

## **Running with Docker Compose**

The project includes a `docker-compose.yml` file for easy multi-container setup (API, frontend with Nginx, and database).

### **How to run:**

1. Make sure Docker is running on your machine.
2. From the project root, run:
   ```sh
   docker-compose up --build
   ```
   This will build and start all services:
   - **respectcounter-api**: .NET backend API
   - **respectcounter-reactapp**: React frontend (built with Vite, served by Nginx)
   - **respectcounter-db**: SQL Server database

   The React frontend is served by Nginx, which also proxies API requests to the backend.

3. Access the app in your browser at [http://localhost:8080](http://localhost:8080)
4. To stop and remove containers:
   ```sh
   docker-compose down
   ```

### **How to run only backend:**
1. Make sure Docker is running on your machine.
2. From the project root, run:
   ```sh
   docker-compose up api --build
   ```
   Or in the detached mode:
   ```sh
   docker-compose up -d api --build
   ```
3. The API will listen on: http://localhost:8080
4. It can be tested thanks to Swagger UI: http://localhost:8080/swagger
4. To stop and remove containers:
   ```sh
   docker-compose down
   ```
---

## **Installation**

Follow these steps to set up the project locally:
1. **Requirements**
    Ensure you have the following installed:
    - [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0)  
    - [**Node.js (v23.0.0)**](https://nodejs.org/)

2. **Clone the repository:**
   ```sh
   git clone https://github.com/SzaroBury/RespectCounter.git
   ```
   
3. **Start the API (manual):**
   ```sh
   dotnet run --project ./RespectCounter.API/
   ```

4. **Start the React app (manual):**
   ```sh
   cd ./RespectCounter.ReactApp/
   npm install
   npm run dev
   ```
   Access the app at http://localhost:3000

---

## Main entities:
- Person
- Activity
- Comment
- Tag
- User
- BaseReaction
- ActivityReaction
- CommentReaction
- PersonReaction
- ActivityTag
- PersonTag

![Entity Relationship Diagram](RespectCounterERD.png)

---

## To-Do List
Backend:
- FluentValidations with Pipeline Behaviour
- Add support for user avatars and images for public figures and activities
- Unit Tests
- Integration Tests
- Make sure that SOLID principles are used
- Add an endpoint for requesting comment replies
- Implement data hiding functionality
- Implement pagination
- Introduce a reporting system for activities and comments
- Implement a system that will give users an ability to propose changes in descriptions of persons and activities.
- Introduce ZLinq
- Add custom exceptions

Frontend:
- Add error popups
- Create a moderation page for verifying public figures and activities
- Improve the home page design
- Redux Toolkit, Zustand or Recoil instead of plain useState
- async with React Query (TanStack Query)
- unit tests (Jest and React Testing Library)
- materialUI or AntDesign
- make sure the app is responsive

DevOps:
- GitHub Actions (or Azure Pipelines) for automatic build and testing
- Deployment on Azure App Service or AWS Elastic Beanstalk
- Serilog