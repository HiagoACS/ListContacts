using System;

public class Program
{
    public static void Main(string[] args)
    {
        Logger logger = new Logger();
        ContactManager contactManager = new ContactManager(logger); // Pass the logger instance to the ContactManager
        while (true)
        {
            Console.WriteLine("1. Create Contact");
            Console.WriteLine("2. View Contacts");
            Console.WriteLine("3. Empty Contacts");
            Console.WriteLine("4. Edit Contacts");
            Console.WriteLine("5. Delete Contact");
            Console.WriteLine("6. Exit");
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
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }
}