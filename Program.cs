using System;

public class Program
{
    public static void Main(string[] args)
    {
        ContactManager contactManager = new ContactManager();
        while (true)
        {
            Console.WriteLine("1. Create Contact");
            Console.WriteLine("2. View Contacts");
            Console.WriteLine("3. Empty Contacts");
            Console.WriteLine("4. Edit Contacts");
            Console.WriteLine("5. Delete Contact");
            Console.WriteLine("6. Export Contacts");
            Console.WriteLine("7. Import Contacts");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    contactManager.CreateContact();
                    break;
                case "2":
                    Console.Clear();
                    contactManager.ListContacts();
                    break;
                case "3":
                    contactManager.ClearContacts();
                    break;
                case "4":
                    contactManager.EditContact();
                    break;
                case "5":
                    contactManager.DeleteContact();
                    break;
                case "6":
                    contactManager.ExportContactsToCsv();
                    break;
                case "7":
                    contactManager.ImportContactsFromCsv();
                    break;
                case "8":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }
}