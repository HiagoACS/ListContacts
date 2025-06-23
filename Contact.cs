using System;

public class Contact
{
	public Guid Id { get; set; } = Guid.NewGuid();
    public string? Nome { get; set; }
	public string? Telefone { get; set; }
	public string? Email { get; set; }

	// Constructors

	// Default constructor
	public Contact() { }

    // Constructor with parameters
    public Contact(string nome, string telefone, string email)
	{
		Nome = nome;
		Telefone = telefone;
		Email = email;
	}
}
