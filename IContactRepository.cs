using System;
using System.Collections.Generic;

public interface IContactRepository
{
    void AddContact(Contact contact); // Method to add a new contact
    List<Contact> GetAllContacts(); // Method to retrieve all contacts
    Contact GetContactById(int id); // Method to retrieve a contact by its ID
    void UpdateContact(Contact contact); // Method to update an existing contact
    void DeleteContact(int id); // Method to delete a contact by its ID
    void DeleteAllContacts(); // Method to delete all contacts
    bool HasContacts(); // Method to check if there are any contacts in the database
    bool ContactExists(string phone, string email); // Method to check if a contact exists by its ID
    bool ContactExistsForEdit(int id, string phone, string email); // Method to check if a contact exists for editing
    void ExportContactsToCsv(string filePath); // Method to export contacts to a CSV file
    void ImportContactsFromCsv(string filePath); // Method to import contacts from a CSV file

}
