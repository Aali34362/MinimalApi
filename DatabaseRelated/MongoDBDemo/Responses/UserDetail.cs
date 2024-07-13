using MongoDBDemo.Models;

namespace MongoDBDemo.Responses;

public class UserDetail : BaseResponse
{
    // Personal Information
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public List<ContactInformations>? contacts { get; set; }
    public List<AddressInformations>? addresses { get; set; }
    public SecurityInformations? security { get; set; }
    public AccountInformations? account { get; set; }
    public Metadatas? metadata { get; set; }
    public SocialInformations? social { get; set; }
}

public class ContactInformations
{
    // Contact Information
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public string? UserPhone { get; set; }
    public string? UserPhoneNumber { get; set; }
}
public class AddressInformations
{
    // Address Information
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}
public class SecurityInformations
{
    // Security Information
    public string? UserPassword { get; set; } // Consider removing this and using hashed version
    public string? UserPasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public string? SecurityQuestion { get; set; }
    public string? SecurityAnswer { get; set; }
}
public class AccountInformations
{
    // Account Information
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEndDateUtc { get; set; }
}
public class Metadatas
{
    // Metadata
    public DateTime? LastLoginDate { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public List<Claim> Claims { get; set; } = new List<Claim>();
}
public class SocialInformations
{
    // Social Information
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
}
