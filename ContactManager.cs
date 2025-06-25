using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using ConsoleContactManager.Interfaces; // Namespace for the interface
using ConsoleContactManager.Models; // Namespace for the Contact model
using ConsoleContactManager.Services; // Namespace for the ContactService

public class ContactManager
{
    private readonly Logger logger;
    private readonly IContactRepository iContactRepository;
    private readonly ContactService service;
    //Constructor
    public ContactManager(Logger logger, IContactRepository iContactRepository)
    {
        this.logger = logger;
        // Initialize the contacts list and load existing contacts from a file

        this.iContactRepository = iContactRepository; // Initialize the contact repository
        this.service = new ContactService(iContactRepository); // Initialize the contact service

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
        if(service.TryAddContact(newContact, out string message))
        {
            logger.WriteLog($"Contact created: {newContact.Name}, Phone: {newContact.Phone}, Email: {newContact.Email}");
        }
        Console.WriteLine(message);

    }

    // List all contacts
    public void ListContacts()
    {
        List<Contact> Contacts = iContactRepository.GetAllContacts(); // Get all contacts from the database
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

    // Edit a contact
    public void EditContact()
    {
        Console.Clear();
        ListContacts();
        // Check if there are any contacts to edit
        if (iContactRepository.HasContacts() != true)
        {
            Console.WriteLine("No contacts available to edit.");
            return;
        }

        // Prompt the user for the name of the contact to edit
        Console.Write("Enter the Id of the contact to edit: ");
        String? input = Console.ReadLine();

        // Validate the input
        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int id))
        {
            Console.Clear();
            Console.WriteLine("Invalid Id. Please enter a valid contact Id.");
            return;
        }

        Contact? contact = iContactRepository.GetContactById(id);
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
        if(service.TryUpdateContact(contact, out string message))
        {
            logger.WriteLog($"Contact updated: {contact.Name}, Phone: {contact.Phone}, Email: {contact.Email}");
        }

        Console.WriteLine(message);
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

        iContactRepository.DeleteAllContacts(); // Delete contacts from the database

        Console.WriteLine("All contacts have been cleared.");
        logger.WriteLog("All contacts cleared.");
    }

    //Delete a contact by name
    public void DeleteContact()
    {
        Console.Clear();
        ListContacts();
        Console.Write("Enter the Id of the contact to delete: ");

        // Validate the input
        string? input = Console.ReadLine();

        Console.Clear();

        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid Id. Please enter a valid contact Id.");
            return;
        }

        if(service.TryDeleteContact(id, out string message, out Contact? contact))
        {
            logger.WriteLog($"Contact deleted: {contact.Name}, Phone: {contact.Phone}, Email: {contact.Email}");
        }
        Console.WriteLine(message);
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
            iContactRepository.ExportContactsToCsv(filePath); // Export contacts to CSV file
            Console.Clear();
            Console.WriteLine($"Contacts exported to {nameArchiveCsv} successfully.");
            logger.WriteLog($"Contacts exported to {nameArchiveCsv}.");
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
                iContactRepository.ImportContactsFromCsv(filePath); // Import contacts from CSV file
                Console.WriteLine($"Contacts imported from {nameArchiveCsv} successfully.");
                logger.WriteLog($"Contacts imported from {nameArchiveCsv}.");
            }
            else
            {
                Console.WriteLine($"The file {nameArchiveCsv} does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while importing contacts: {ex.Message}");
            logger.WriteLog($"An error occurred while importing contacts from {nameArchiveCsv}: {ex.Message}");
        }
    }
}
