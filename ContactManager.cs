using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ContactManager
{
    private readonly Logger logger;
    private readonly ContactRepository contactRepository;
    //Constructor
    public ContactManager(Logger logger)
    {
        this.logger = logger;
        // Initialize the contacts list and load existing contacts from a file

        contactRepository = new ContactRepository(); // Initialize the contact repository

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

        // Check if the contact already exists
        if (contactRepository.ContactExists(newContact.Phone, newContact.Email))
        {
            Console.WriteLine("A contact with the same phone or email already exists.");
            return;
        }

        // Add the new contact to the list and save it
        contactRepository.AddContact(newContact); // Save the contact to the database
        logger.WriteLog($"Contact created: {newContact.Name}, Phone: {newContact.Phone}, Email: {newContact.Email}");
        Console.WriteLine("Contact created successfully!");

    }

    public void ListContacts()
    {
        List<Contact> Contacts = contactRepository.GetAllContacts(); // Get all contacts from the database
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

        contactRepository.DeleteAllContacts(); // Delete contacts from the database

        Console.WriteLine("All contacts have been cleared.");
        logger.WriteLog("All contacts cleared.");
    }

    // Edit a contact
    public void EditContact()
    {
        Console.Clear();
        ListContacts();
        // Check if there are any contacts to edit
        if (contactRepository.HasContacts() != true)
        {
            Console.WriteLine("No contacts available to edit.");
            return;
        }

        // Prompt the user for the name of the contact to edit
        Console.Write("Enter the Id of the contact to edit: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Contact? contact = contactRepository.GetContactById(id);
        Console.Clear();
        if (contact == null)
        {
            Console.WriteLine("Contact not found.");
            return;
        }

        // Display the current details of the contact and prompt for new values
        Console.WriteLine($"Editing contact: {contact.Name}");

        logger.WriteLog($"Editing contact: {contact.Name}");

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
        Console.Clear();
        //Check if the updated contact already exists
        if (contactRepository.ContactExistsForEdit(contact.Id, contact.Phone, contact.Email))
        {
            Console.WriteLine("A contact with the same phone or email already exists.");
            return;
        }
        // Save the updated contacts list to the file
        contactRepository.UpdateContact(contact); // Update the contact in the database

        logger.WriteLog($"Contact edited: {contact.Name}, Phone: {contact.Phone}, Email: {contact.Email}");

    }

    //Delete a contact by name
    public void DeleteContact()
    {
        Console.Clear();
        ListContacts();
        Console.Write("Enter the Id of the contact to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Contact? contact = contactRepository.GetContactById(id);
        Console.Clear();
        if (contact != null)
        {
            contactRepository.DeleteContact(id);
            logger.WriteLog($"Contact deleted: {contact.Name}, Phone: {contact.Phone}, Email: {contact.Email}");
            Console.WriteLine($"Contact deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Contact not found.");
        }
    }
    /*
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
            logger.WriteLog($"Contacts exported to {nameArchiveCsv} successfully.");
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.WriteLine($"An error occurred while exporting contacts: {ex.Message}");
            logger.WriteLog($"An error occurred while exporting contacts to {nameArchiveCsv}: {ex.Message}");
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
            logger.WriteLog($"Importing contacts from {nameArchiveCsv}");
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
                logger.WriteLog($"The file {nameArchiveCsv} does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while importing contacts: {ex.Message}");
            logger.WriteLog($"An error occurred while importing contacts from {nameArchiveCsv}: {ex.Message}");
        }
    }
    */
}
