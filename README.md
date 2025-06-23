
# Console Contact Manager

A simple console application written in C# to manage contacts.  
This project implements a complete basic **CRUD** (Create, Read, Update, Delete) system for contact management, with data saved in a JSON file.

## Features ✅

- **Create**: Add new contacts
- **Read**: List all saved contacts
- **Update**: Edit existing contacts by name
- **Delete**: Remove specific contacts by name or clear the entire contact list
- Persist contact data in a local JSON file (`contacts.json`)

## Getting Started 🚀

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed
- Visual Studio (recommended) or any other C# IDE

### Running the Application

1. Clone the repository:

   ```bash
   git clone https://github.com/HiagoACS/ListContacts.git
   ```

2. Open the solution in Visual Studio.

3. Build and run the project.

### Notes

- All contacts are saved locally in a JSON file (`contacts.json`).
- The `contacts.json` file is ignored in version control (`.gitignore`) to prevent personal data from being pushed to the repository.

## Possible Next Steps 🚧

- Create unit tests
- Migrate to a graphical interface (WinForms/WPF)
- Persist data in a database (e.g., SQLite)

## Contributing 🤝

Feel free to submit issues or pull requests to help improve this project.

