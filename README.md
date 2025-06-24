# Console Contact Manager

A simple console-based Contact Manager written in C#.

This project allows users to create, view, update, and delete contacts directly from the console, using **SQLite** as the data storage backend.

---

## 📌 Features

✅ Create new contacts  
✅ List all contacts  
✅ Edit an existing contact  
✅ CSV Export and Import  
✅ Delete a contact  
✅ Prevent duplicate contacts (by phone number or email)  
✅ Confirm deletion before clear the database  
✅ Automatically saves contacts in an SQLite database  
✅ Basic input validation  
✅ Logs important actions (e.g., contact creation, deletion) to a text file

---

## ✅ Current Data Storage

- All contacts are now stored in a local SQLite database (`contacts.db`), created automatically at runtime.
- The application uses the `Microsoft.Data.Sqlite` package for database operations.

---

## 🚫 Temporarily Removed Features

- Search for a contact by name or ID 
(These will be re-implemented later, adapted to work directly with the SQLite database.)

---

## 🛠️ How to Run

1. Clone the repository
2. Open the solution in Visual Studio
3. Make sure the `Microsoft.Data.Sqlite` NuGet package is installed
4. Build and run the application

---

## 📝 Future Improvements

- Implement unit tests  
- Implement pagination or filtering for large datasets  
- Improve error handling  
- Add DateCreated field for each contact  

---

## 📅 Project Status

The project is in an educational / learning phase, focusing on clean code structure, basic CRUD operations, and working with a database.

---

## 📣 Contributions

This project is for learning purposes only. Feel free to fork or suggest improvements.

