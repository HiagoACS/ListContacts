using System;

//Not use Services namespace here, as this is the model definition
namespace ConsoleContactManager.Models;

public class Contact
{
	public int Id { get; set; }
    public string? Name { get; set; }
	public string? Phone { get; set; }
	public string? Email { get; set; }

	// Constructors

	// Default constructor
	public Contact() { }

    // Constructor with parameters
    public Contact(string name, string phone, string email)
	{
		Name = name;
		Phone = phone;
		Email = email;
	}
}
