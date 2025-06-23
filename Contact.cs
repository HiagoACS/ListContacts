using System;

public class Contact
{
	public Guid Id { get; set; } = Guid.NewGuid();
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
