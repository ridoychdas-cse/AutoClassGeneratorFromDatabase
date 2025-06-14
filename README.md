# 🔧 BOL Class Generator & SQL Script Runner (C# Console App)

## 📝 Project Description
A lightweight and powerful **C# Console Application** that automates the generation of **Business Object Layer (BOL) classes** directly from a **SQL Server database schema**, and executes predefined **SQL scripts** such as stored procedures. 
Perfect for developers working in **layered architecture** projects where clean, consistent, and automatically generated C# classes are essential.
---

## 📦 Features
- 🔄 Auto-generate C# class files (BOLs) from database tables
- 🧠 Smart type mapping from SQL to C# (including nullable types)
- 🏗️ Executes SQL script on first run (e.g., stored procedures)
- 🔎 Code-friendly formatting with comments and region tags
- ✅ Skips existing objects and handles drop-create logic
- 🛠️ Easily customizable for any enterprise-level .NET project
---

## 🛠️ Technologies Used
- **Language:** C# (.NET 6+ compatible)
- **Database:** Microsoft SQL Server
- **Access:** ADO.NET (`SqlConnection`, `SqlCommand`)
- **Helper:** Newtonsoft.Json for class decoration
- **IDE:** Visual Studio / Rider
---

## 📁 Folder Structure
│
├── Database Scripts/
│ └── Create_Procedure.sql # SQL for auto-generating BOL class content
│
├── BOL/
│ └── [GeneratedClasses].cs # Output C# class files
│
├── Program.cs # Entry point of the console app
├── DataManager.cs # Contains logic to run SQL & generate code
├── README.md # Project documentation
---

## 🧪 On Running the App:
1. Validates if the SQL script (`Create_Procedure.sql`) exists
2. Executes the script to create or drop+create the stored procedure
3. Stored procedure returns BOL property definitions with types
4. Program dynamically builds `.cs` class files for each table
5. Console output confirms success or any issues
---


## 🚀 How to Use This Project
### 1️⃣ Clone the repository:
```bash
git clone https://github.com/ridoychdas-cse/AutoClassGeneratorFromDatabase

2️⃣ Setup connection string in DataManager.cs:
const string connectionString = @"Server=localhost;Database=YourDb;User Id=sa;Password=yourPassword;";

3️⃣ Verify SQL Script
Ensure Create_Procedure.sql is present in the Database Scripts folder.

4️⃣ Run the app



