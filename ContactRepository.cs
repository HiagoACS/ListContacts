using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;

public class ContactRepository : IContactRepository // Implementation of the IContactRepository interface
{
	private readonly string connectionString;

    public ContactRepository()
    {
        string projectRoot = AppDomain.CurrentDomain.BaseDirectory; // Get the base directory of the application
        connectionString = $"Data Source={System.IO.Path.Combine(projectRoot, "contacts.db")}";

        CreateDatabase();
    }

    private void CreateDatabase() // Create the database and the Contacts table if they do not exist
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open(); // Open the connection to the SQLite database

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Contacts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL ,
                    Phone TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL UNIQUE
                )";
            command.ExecuteNonQuery();
        }
    }

    public void AddContact(Contact contact) // Add a new contact to the database
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Contacts (Name, Phone, Email)
                VALUES ($name, $phone, $email)";
            command.Parameters.AddWithValue("$name", contact.Name);
            command.Parameters.AddWithValue("$phone", contact.Phone);
            command.Parameters.AddWithValue("$email", contact.Email);
            command.ExecuteNonQuery();
        }
    }

    public List<Contact> GetAllContacts() // Retrieve all contacts from the database
    {
        var contacts = new List<Contact>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Phone, Email FROM Contacts";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    contacts.Add(new Contact
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Email = reader.GetString(3)
                    });
                }
            }
        }
        return contacts;
    }

    public Contact GetContactById(int id) // Retrieve a contact by its ID
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Phone, Email FROM Contacts WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Contact
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Email = reader.GetString(3)
                    };
                }
            }
        }
        return null; // Return null if no contact is found
    }

    public void UpdateContact(Contact contact) // Update an existing contact
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Contacts
                SET Name = $name, Phone = $phone, Email = $email
                WHERE Id = $id";
            command.Parameters.AddWithValue("$id", contact.Id);
            command.Parameters.AddWithValue("$name", contact.Name);
            command.Parameters.AddWithValue("$phone", contact.Phone);
            command.Parameters.AddWithValue("$email", contact.Email);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteContact(int id) // Delete a contact by its ID
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Contacts WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteAllContacts() // Delete all contacts from the database
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Delete all contacts
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Contacts";
            command.ExecuteNonQuery();

            // Reset the auto-increment ID
            command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Contacts'";
            command.ExecuteNonQuery();
        }
    }

    public bool HasContacts() // Check if there are any contacts in the database
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Contacts";
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }

    public bool ContactExists(string phone, string email) // Check if a contact with the same phone or email already exists
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Contacts
                WHERE Phone = $phone OR Email = $email";
            command.Parameters.AddWithValue("$phone", phone);
            command.Parameters.AddWithValue("$email", email);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }

    public bool ContactExistsForEdit(int id, string phone, string email) // Check if a contact with the same phone or email already exists, excluding the contact being edited
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Contacts
                WHERE (Phone = $phone OR Email = $email) AND Id != $id";
            command.Parameters.AddWithValue("$phone", phone);
            command.Parameters.AddWithValue("$email", email);
            command.Parameters.AddWithValue("$id", id);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }

    public void ExportContactsToCsv(string filePath) // Export all contacts to a CSV file
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Name, Phone, Email FROM Contacts";
            using (var reader = command.ExecuteReader())
            {
                using (var writer = new System.IO.StreamWriter(filePath))
                {
                    writer.WriteLine("Name,Phone,Email"); // Write CSV header
                    while (reader.Read())
                    {
                        writer.WriteLine($"{reader.GetString(0)},{reader.GetString(1)}, {reader.GetString(2)}");
                    }
                }
            }
        }
    }

    public void ImportContactsFromCsv(string filePath) // Import contacts from a CSV file
    {
        using (var reader = new System.IO.StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    var contact = new Contact
                    {
                        Name = parts[0],
                        Phone = parts[1],
                        Email = parts[2]
                    };
                    AddContact(contact); // Add the contact to the database
                }
            }
        }
    }
}
