# 🎓 Student API — ASP.NET Core 3-Tier Architecture

Student API is a fully featured **RESTful Web API** built using **ASP.NET Core** and organized using a clean **3-Tier Architecture**, with direct SQL Server integration using ADO.NET.

This project provides all basic CRUD operations for managing students, including image upload and retrieval, with a scalable structure ready for future extensions.

---

## 📸 Swagger Screenshot

![Swagger Screenshot](./Images/SwaggerScreenshot.png)  
*(Replace this image path after uploading your Swagger screenshot)*

---

## 🔥 Features

- ✔ Get all students  
- ✔ Get passed students  
- ✔ Get average grade  
- ✔ Get student by ID  
- ✔ Add new student  
- ✔ Update existing student  
- ✔ Delete student  
- ✔ Upload student image  
- ✔ Retrieve saved image  

---

## 🧱 Project Architecture — 3-Tier Structure

┌──────────────────────────┐
│ Presentation Layer │ → StudentAPI (Controllers)
└──────────────────────────┘
┌──────────────────────────┐
│ Business Layer │ → StudentAPIBusinessLayer (DTOs + Managers)
└──────────────────────────┘
┌──────────────────────────┐
│ Data Access Layer │ → StudentDataAccessLayer (ADO.NET + SQL)
└──────────────────────────┘

yaml
Copy code

---

## 🗂 Folder Structure

StudentSuite/
│
├── StudentAPI/ # Web API Layer
│ ├── Controllers/
│ ├── Program.cs
│ └── appsettings.json
│
├── StudentAPIBusinessLayer/ # Business Logic Layer
│ ├── DTOs/
│ └── StudentManager.cs
│
└── StudentDataAccessLayer/ # Data Access Layer
├── Entitys/
└── StudentData.cs

yaml
Copy code

---

## 🔗 API Endpoints

### 📍 Student Endpoints

| Method | Endpoint                        | Description              |
|--------|----------------------------------|--------------------------|
| GET    | `/api/Students/All`              | Get all students         |
| GET    | `/api/Students/Passed`           | Get passed students      |
| GET    | `/api/Students/AverageGrade`     | Get average grade        |
| GET    | `/api/Students/{id}`             | Get student by ID        |
| POST   | `/api/Students`                  | Add new student          |
| PUT    | `/api/Students/{id}`             | Update student           |
| DELETE | `/api/Students/{id}`             | Delete student           |

---

### 📸 Image Endpoints

| Method | Endpoint                                | Description         |
|--------|-------------------------------------------|---------------------|
| POST   | `/api/Students/UploadImage`               | Upload image        |
| GET    | `/api/Students/GetImage/{imageName}`      | Retrieve image      |

---

## 🧪 Example Request — Add Student

### **POST — `/api/Students`**
```json
{
  "name": "Haider",
  "age": 22,
  "grade": 90
}
Response (201):

json
Copy code
{
  "id": 12,
  "name": "Haider",
  "age": 22,
  "grade": 90
}
🗄 Stored Procedures Used
GetAllStudent

GetPassedStudent

GetAverageGrade

GetStudentById

AddStudent

UpdateStudent

DeleteStudent

⚙️ Technologies Used
ASP.NET Core Web API

C#

SQL Server

ADO.NET

3-Tier Architecture

DTO Pattern

Swagger Documentation

File Uploading

🚀 How to Run
Open the project in Visual Studio

Update the database connection string in appsettings.json:

json
Copy code
"ConnectionStrings": {
  "StudentDB": "Server=.;Database=StudentDB;Trusted_Connection=True;"
}
Execute all stored procedures in SQL Server

Run the project

Open Swagger at:

bash
Copy code
https://localhost:{port}/swagger
👤 Author
Haider Mohammed
Developer — C#, SQL, Web API, 3-Tier Architecture