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
            Console.WriteLine($"Name: {contact.Nome}, Phone: {contact.Telefone}, Email: {contact.Email}");
        }
    }

    // Empty the contacts list
    public void ClearContacts()
    {
        Contacts.Clear();
        SaveContacts();
        Console.WriteLine("All contacts have been cleared.");
    }
}
