# 🗳️ Survey Basket API

Survey Basket is a **RESTful API** designed to manage surveys, users, questions, answers, participation, and results.  
It provides a complete backend solution for building and managing polls with token-based authentication and user management features.

---

## 🚀 Features

### 🔐 Account Management
- User registration & account confirmation  
- Secure login with tokens (JWT)  
- Change password  
- Update profile  

### 👤 User Management
- Add & update users  
- Manage roles & permissions  
- Lock / Unlock accounts  
- Reset password  

### 📊 Polls
- List all polls  
- Add, update, and delete polls  
- Send notifications to participants  

### ❓ Questions
- List questions  
- Add, update, and delete questions  

### 📝 Answers
- List answers  
- Add, update, and delete answers  

### 🧑‍🤝‍🧑 Participation
- View available polls  
- Submit participation  

### 📈 Results
- Display and manage survey results  

---

## 🛠️ Tech Stack
- **Backend:** ASP.NET Core Web API  
- **Database:** SQL Server  
- **Authentication:** JWT (JSON Web Token)  
- **Version Control:** Git  
- **Documentation:** Swagger / OpenAPI  

---

## 📂 Architecture Overview
The system is modular and consists of several core services:
- **Account Management Service** → Handles user registration, login, and profile management.  
- **User Service** → Manages users, roles, and permissions.  
- **Polls Service** → CRUD operations for polls with notification support.  
- **Questions Service** → CRUD operations for survey questions.  
- **Answers Service** → CRUD operations for survey answers.  
- **Participation Service** → Allows users to participate in polls.  
- **Results Service** → Displays and manages poll results.  

---

## ⚡ Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/morsy777/Survey-Basket-API.git
cd Survey-Basket-API
```

### 2. Configure Database

Update the database connection string in `appsettings.json`.

### 3. Apply Migrations

```bash
dotnet ef database update
```

### 4. Run the Project

```bash
dotnet run
```

### 5. API Documentation

Once the project is running, the API documentation will be available at:

```
https://localhost:{port}/swagger
```

---


## 👨‍💻 Developer

**Mohamed Morsi**  

- GitHub: [morsy777](https://github.com/morsy777)  
- Azure DevOps: [EngMohamedMorsi7](https://dev.azure.com/EngMohamedMorsi7)

---

