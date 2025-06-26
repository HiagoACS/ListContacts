# 📇 Console Contact Manager

A simple console-based Contact Manager written in C#.  
This project allows users to create, view, update, delete, export, and import contacts directly from the console, using **SQLite** as the data storage backend.

---

## 📌 Features

✅ Create new contacts  
✅ List all contacts  
✅ Edit an existing contact  
✅ Delete a contact by ID  
✅ Confirm before deleting all contacts  
✅ CSV Export and Import  
✅ Prevent duplicate contacts (by phone number or email)  
✅ Automatically saves contacts in an SQLite database  
✅ Input validation and proper error messages  
✅ Logs important actions (creation, update, deletion, import/export, etc.) to a text file  
✅ Separation of concerns using **Repository**, **Service**, and **Manager** layers  
✅ Interface abstraction with `IContactRepository`

---

## ✅ Data Storage

- All contacts are stored in a local SQLite database (`contacts.db`) automatically created at runtime.
- Uses the `Microsoft.Data.Sqlite` package for database operations.
- Contact table fields:
  - `Id` (auto-increment)
  - `Name`
  - `Phone` (unique)
  - `Email` (unique)

---

## 📤 CSV Support

- **Export:** Writes all contacts to `contacts.csv` in the root directory.
- **Import:** Reads contacts from a CSV file and skips duplicates based on phone/email.

---

## 🧱 Architecture Overview

Program.cs (Entry Point)  
│  
├── ContactManager.cs --> Handles all user interface and console logic  
│  
├── Services/  
│ └── ContactService.cs --> Business logic (validations, conflict checks)  
│  
├── Repositories/  
│ └── ContactRepository.cs --> SQLite operations  
│  
├── Interfaces/  
│ └── IContactRepository.cs --> Defines repository contract  
│  
├── Models/  
│ └── Contact.cs --> Contact entity  
│  
└── Logger.cs --> Logs messages to log.txt  


---

## 🛠️ How to Run

1. Clone the repository  
2. Open the solution in **Visual Studio**  
3. Ensure the NuGet package `Microsoft.Data.Sqlite` is installed  
4. Build and run the application  
5. Follow the menu in the console

---

## 📝 Sugestions to Future Improvements

- Unit tests  
- UI with WinForms or WPF (optional)

---

## 📅 Project Status - 100%

The project is in an **educational/learning** stage, focused on best practices in architecture, data access patterns, and clean code principles.

---

## 📣 Contributions

This project is for learning purposes, but suggestions and pull requests are welcome!
