using Ardalis.GuardClauses;
using System.ComponentModel.DataAnnotations;

namespace Elearninig.Packages.Email.Base.Models;

public class EmailAddressModel
{
    public string Email { get; private set; }
    public string? Name { get; private set; }

    public EmailAddressModel(string email, string? name)
    {
        var emailIsValid = new EmailAddressAttribute().IsValid(email);
        if (!emailIsValid) throw new ArgumentException($"Email '{email}' is not a valid email.");

        Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
        Name = name;
    }
}