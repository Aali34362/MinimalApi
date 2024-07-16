namespace MongoDBDemo.Models;

public class User : BaseEntity
{
    // Personal Information
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    [BsonIgnoreIfNull]
    public List<ContactInformation>? contacts { get; set; }
    [BsonIgnoreIfNull]
    public List<AddressInformation>? addresses { get; set; }
    [BsonIgnoreIfNull]
    public SecurityInformation? security { get; set; }
    [BsonIgnoreIfNull]
    public AccountInformation? account { get; set; }
    [BsonIgnoreIfNull]
    public Metadata? metadata { get; set; }
    [BsonIgnoreIfNull]
    public SocialInformation? social { get; set; }
}

public class ContactInformation 
{
    // Contact Information
    public string? UserEmail { get; set; }
    public string? UserPhone { get; set; }
    public string? UserPhoneNumber { get; set; }
}
public class AddressInformation
{
    // Address Information
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}
public class SecurityInformation
{
    // Security Information
    public string? UserPassword { get; set; } // Consider removing this and using hashed version
    public string? UserPasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public string? SecurityQuestion { get; set; }
    public string? SecurityAnswer { get; set; }
}
public class AccountInformation
{
    // Account Information
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEndDateUtc { get; set; }
}
public class Metadata
{
    // Metadata
    public DateTime? LastLoginDate { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public List<Claim> Claims { get; set; } = new List<Claim>();
}
public class SocialInformation
{
    // Social Information
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
}
