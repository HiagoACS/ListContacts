using System;

using ConsoleContactManager.Interfaces; // Namespace for the interface
using ConsoleContactManager.Models; // Namespace for the Contact model

namespace ConsoleContactManager.Services
{

    public class ContactService
    {

	    private readonly IContactRepository contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public bool TryAddContact(Contact contact, out string message) { 
            if (string.IsNullOrEmpty(contact.Name) || string.IsNullOrEmpty(contact.Phone) || string.IsNullOrEmpty(contact.Email))
            {
                message = "All fields are required. Contact creation failed.";
                return false;
            }
            if (contactRepository.ContactExists(contact.Phone, contact.Email))
            {
                message = "A contact with the same phone or email already exists.";
                return false;
            }
            contactRepository.AddContact(contact);
            message = "Contact created successfully!";
            return true;
        }

        public bool TryUpdateContact(Contact contact, out string message)
        {
            if (string.IsNullOrEmpty(contact.Name) || string.IsNullOrEmpty(contact.Phone) || string.IsNullOrEmpty(contact.Email))
            {
                message = "All fields are required. Contact update failed.";
                return false;
            }
            if (contactRepository.ContactExistsForEdit(contact.Id, contact.Phone, contact.Email))
            {
                message = "A contact with the same phone or email already exists.";
                return false;
            }
            contactRepository.UpdateContact(contact);
            message = $"Contact {contact.Id} updated successfully!";
            return true;
        }

        public bool TryDeleteContact(int id, out string message, out Contact? contact)
        {
            contact = contactRepository.GetContactById(id);
            if (contact == null)
            {
                message = $"No contact with Id:{id}.";
                return false;
            }
            contactRepository.DeleteContact(id);
            message = $"Contact with Id:{id} deleted successfully!";
            return true;
        }
    }

}

