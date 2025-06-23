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
        newContact.Nome = Console.ReadLine();

        Console.Write("Enter phone: ");
        newContact.Telefone = Console.ReadLine();

        Console.Write("Enter email: ");
        newContact.Email = Console.ReadLine();

        //Validate the contact details
        if (string.IsNullOrEmpty(newContact.Nome) || string.IsNullOrEmpty(newContact.Telefone) || string.IsNullOrEmpty(newContact.Email))
        {
            Console.WriteLine("All fields are required. Contact creation failed.");
            return;
        }

        // Add the new contact to the list and save it
        AddContact(newContact);
        Console.WriteLine("Contact created successfully!");

        // Save the updated contacts list to the file
        SaveContacts();
    }

    // Save contacts to a JSON file
    public void SaveContacts()
    {
        string json = JsonSerializer.Serialize(Contacts);
        File.WriteAllText(filePath, json);
    }


    // Load contacts from a JSON file
    public void getContactArchive()
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
            Console.WriteLine("No contacts found.");
            return;
        }
        Console.WriteLine("Contacts:");
        foreach (var contact in Contacts)
        {
            Console.WriteLine($"ID: {contact.Id} \nName: {contact.Nome}, Phone: {contact.Telefone}, Email: {contact.Email}");
        }
    }

    // Empty the contacts list
    public void ClearContacts()
    {
        // Confirm with the user before clearing
        Console.Write("Are you sure you want to clear all contacts? (y/n): ");
        string? confirmation = Console.ReadLine();
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

        // Check if there are any contacts to edit
        if (Contacts.Count == 0)
        {
            Console.WriteLine("No contacts available to edit.");
            return;
        }

        // Prompt the user for the name of the contact to edit
        Console.Write("Enter the name of the contact to edit: ");
        Contact? contact = FindContactByName(Contacts);
        if (contact == null)
        {
            Console.WriteLine("Contact not found.");
            return;
        }

        // Display the current details of the contact and prompt for new values
        Console.WriteLine($"Editing contact: {contact.Nome}");

        Console.Write("Enter new name (leave empty to keep current): ");
        string? newName = Console.ReadLine();
        if (!string.IsNullOrEmpty(newName))
        {
            contact.Nome = newName;
        }
        Console.Write("Enter new phone (leave empty to keep current): ");
        string? newPhone = Console.ReadLine();
        if (!string.IsNullOrEmpty(newPhone))
        {
            contact.Telefone = newPhone;
        }
        Console.Write("Enter new email (leave empty to keep current): ");
        string? newEmail = Console.ReadLine();
        if (!string.IsNullOrEmpty(newEmail))
        {
            contact.Email = newEmail;
        }
        // Save the updated contacts list to the file
        SaveContacts();
    }

    // Find a contact by name
    public Contact? FindContactByName(List<Contact> Contacts)
    {
        // Prompt the user for the name of the contact to find
        Console.WriteLine("\nContact: ");
        string? name = Console.ReadLine();

        // Find a list of contacts by name, ignoring case
        // Using While loop to handle multiple matches
        while (true)
        {
            Contact? contact = Contacts.Find(c => c.Nome?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
            if (contact != null)
            {
                return contact;
            }
            else
            {
                Console.WriteLine("Contact not found. Please try again.");
                ListContacts();
                name = Console.ReadLine();
            }
        }
    }

    //Delete a contact by name
    public void DeleteContact()
    {
        Console.Write("Enter the name of the contact to delete: ");
        Contact? contact = FindContactByName(Contacts);
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
}
