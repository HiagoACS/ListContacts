using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ContactManager
{
    private List<Contact> Contacts { get; set; } = new List<Contact>();
    private string filePath = "contacts.json";

    //Constructor
    public ContactManager()
    {
        // Initialize the contacts list and load existing contacts from a file
        Contacts = new List<Contact>();
        getContactArchive();
    }

    // Properties
    public void AddContact(Contact contact)
    {
        Contacts.Add(contact);
    }
    public void RemoveContact(Contact contact)
    {
        Contacts.Remove(contact);
    }
    public List<Contact> GetContacts()
    {
        return Contacts;
    }

    // Create a new contact
    public void CreateContact()
    {
        Contact newContact = new Contact();

        Console.Write("Enter name: ");
        newContact.Name = Console.ReadLine();

        Console.Write("Enter phone: ");
        newContact.Phone = Console.ReadLine();

        Console.Write("Enter email: ");
        newContact.Email = Console.ReadLine();

        Console.Clear();
        //Validate the contact details
        if (string.IsNullOrEmpty(newContact.Name) || string.IsNullOrEmpty(newContact.Phone) || string.IsNullOrEmpty(newContact.Email))
        {
            Console.WriteLine("All fields are required. Contact creation failed.");
            return;
        }
        // Check if a contact with the same phone already exists
        if (Contacts.Exists(c => c.Phone == newContact.Phone))
        {
            Console.WriteLine("A contact with this phone number already exists. Contact creation failed.");
            return;
        }


        // Add the new contact to the list and save it
        AddContact(newContact);
        Console.WriteLine("Contact created successfully!");

        // Save the updated contacts list to the file
        SaveContacts();
    }

    public void CreateContact(string name, string phone, string email)
    {
        Contact newContact = new Contact();

        newContact.Name = name;
        newContact.Phone = phone;
        newContact.Email = email;

        //Validate the contact details
        if (string.IsNullOrEmpty(newContact.Name) || string.IsNullOrEmpty(newContact.Phone) || string.IsNullOrEmpty(newContact.Email))
        {
            Console.WriteLine("Contact don't have all fields. Contact creation failed.");
            return;
        }
        // Check if a contact with the same phone already exists
        if (Contacts.Exists(c => c.Phone == newContact.Phone))
        {
            Console.WriteLine("A contact with this phone number already exists. Contact creation failed.");
            return;
        }


        // Add the new contact to the list and save it
        AddContact(newContact);
        Console.WriteLine($"Contact {newContact.Name} added!");

        // Save the updated contacts list to the file
        SaveContacts();
    }
    // Save contacts to a JSON file
    private void SaveContacts()
    {
        // Order contacts by name before saving
        Contacts = Contacts.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ToList();
        string json = JsonSerializer.Serialize(Contacts);
        File.WriteAllText(filePath, json);
    }


    // Load contacts from a JSON file
    private void getContactArchive()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText("contacts.json");
            Contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        }
    }

    // List all contacts
    public void ListContacts()
    {
        if (Contacts.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No contacts found.");
            return;
        }
        Console.WriteLine("Contacts:");
        foreach (var contact in Contacts)
        {
            Console.WriteLine($"ID: {contact.Id} \nName: {contact.Name}, Phone: {contact.Phone}, Email: {contact.Email}");
        }
    }

    // Empty the contacts list
    public void ClearContacts()
    {
        // Confirm with the user before clearing
        Console.Write("Are you sure you want to clear all contacts? (y/n): ");
        string? confirmation = Console.ReadLine();
        Console.Clear();
        if (confirmation?.ToLower() != "y")
        {
            Console.WriteLine("Operation cancelled.");
            return;
        }
        Contacts.Clear();
        SaveContacts();

        Console.WriteLine("All contacts have been cleared.");
    }

    // Edit a contact
    public void EditContact()
    {
        Console.Clear();
        ListContacts();
        // Check if there are any contacts to edit
        if (Contacts.Count == 0)
        {
            Console.WriteLine("No contacts available to edit.");
            return;
        }

        // Prompt the user for the name of the contact to edit
        Console.Write("Enter the name of the contact to edit: ");
        Contact? contact = FindContact(Contacts);
        if (contact == null)
        {
            Console.WriteLine("Contact not found.");
            return;
        }

        // Display the current details of the contact and prompt for new values
        Console.WriteLine($"Editing contact: {contact.Name}");

        Console.Write("Enter new name (leave empty to keep current): ");
        string? newName = Console.ReadLine();
        if (!string.IsNullOrEmpty(newName))
        {
            contact.Name = newName;
        }
        Console.Write("Enter new phone (leave empty to keep current): ");
        string? newPhone = Console.ReadLine();
        if (!string.IsNullOrEmpty(newPhone))
        {
            contact.Phone = newPhone;
        }
        Console.Write("Enter new email (leave empty to keep current): ");
        string? newEmail = Console.ReadLine();
        if (!string.IsNullOrEmpty(newEmail))
        {
            contact.Email = newEmail;
        }
        // Save the updated contacts list to the file
        Console.Clear();
        SaveContacts();
    }

    // Find a contact by name
    private Contact? FindContact(List<Contact> Contacts)
    {
        Console.Clear();
        Console.WriteLine("Search contact with Id or Name:");
        Console.WriteLine("1. Search by Name");
        Console.WriteLine("2. Search by Id");
        Console.Write("Choose an option (1 or 2): ");
        string? option = Console.ReadLine();
        Contact? contact = null;
        // Find a list of contacts by name, ignoring case
        // Using While loop to handle multiple matches
        while (true)
        {
            // Get the name or ID to search for
            if (option == "1") 
            { 
                Console.Write("Enter the name of the contact: ");
                string name = Console.ReadLine();
                contact = Contacts.Find(c => c.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
            }
            else if (option == "2") 
            {
                Console.Write("Enter the ID of the contact: ");
                string id = Console.ReadLine();
                contact = Contacts.Find(c => c.Id.ToString().Equals(id, StringComparison.OrdinalIgnoreCase) == true);
            }


            // If a contact is found, return it
            if (contact != null)
            {
                return contact;
            }

            // If no contact is found, prompt the user to try again
            else
            {
                Console.Clear();
                Console.WriteLine("Contact not found. Please try again.");
                ListContacts();
                Console.WriteLine("1. Search by Name");
                Console.WriteLine("2. Search by Id");
                option = Console.ReadLine();
            }
        }
    }

    // Find a contact by ID


    //Delete a contact by name
    public void DeleteContact()
    {
        Console.Clear();
        Console.Write("Enter the name of the contact to delete: ");
        Contact? contact = FindContact(Contacts);
        if (contact != null)
        {
            Contacts.Remove(contact);
            SaveContacts();
            Console.WriteLine($"Contact deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Contact not found.");
        }
    }

    // Export contacts to a csv file
    public void ExportContactsToCsv()
    {

        string nameArchiveCsv = "contacts.csv";
        // Define the path to the CSV file
        string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..");
        string filePath = Path.Combine(projectRoot, nameArchiveCsv);

        //Try/Catch to save archive
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Id,Name,Phone,Email");
                foreach (var contact in Contacts)
                {
                    writer.WriteLine($"{contact.Id},{contact.Name},{contact.Phone},{contact.Email}");
                }
            }
            Console.Clear();
            Console.WriteLine($"Contacts exported to {nameArchiveCsv} successfully.");
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.WriteLine($"An error occurred while exporting contacts: {ex.Message}");
        }
    }

    // Import contacts from a csv file
    public void ImportContactsFromCsv()
    {
        Console.WriteLine("Enter the name of the CSV file to import (default is contacts.csv): ");

        string nameArchiveCsv = Console.ReadLine();
        if (string.IsNullOrEmpty(nameArchiveCsv))
        {
            nameArchiveCsv = "contacts.csv"; // Default file name
        }
        // Define the path to the CSV file
        string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..");
        string filePath = Path.Combine(projectRoot, nameArchiveCsv);
        // Try/Catch to load archive
        Console.Clear();
        try
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Id,Name,Phone,Email")) continue; // Skip header line
                        var parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            // Create a new contact with the data from the CSV file, parts[0] is Id, parts[1] is Name, parts[2] is Phone, parts[3] is Email
                            CreateContact(parts[1], parts[2], parts[3]); // CreateContact method expects Name, Phone, Email
                        }
                    }
                }
                Console.WriteLine($"Contacts imported from {nameArchiveCsv} successfully.");
            }
            else
            {
                Console.WriteLine($"The file {nameArchiveCsv} does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while importing contacts: {ex.Message}");
        }
    }
}
