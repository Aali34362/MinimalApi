using Bogus;

namespace MongoDBDemo.MainOperations;

public class UserOperations(IMongoRepository mongoRepository)
{
    private readonly IMongoRepository _mongoRepository = mongoRepository ?? throw new Exception();
    public async Task CreateUser()
    {
        var faker = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
            .RuleFor(u => u.Gender, f => f.PickRandom(new[] { "Male", "Female" }))
            .RuleFor(u => u.contacts, f => new List<ContactInformation>
            {
                new ContactInformation
                {
                    UserEmail = f.Internet.Email(),
                    UserPhone = f.Phone.PhoneNumber(),
                    UserPhoneNumber = f.Phone.PhoneNumber()
                }
            })
            .RuleFor(u => u.addresses, f => new List<AddressInformation>
            {
                new AddressInformation
                {
                    StreetAddress = f.Address.StreetAddress(),
                    City = f.Address.City(),
                    State = f.Address.State(),
                    Country = f.Address.Country(),
                    PostalCode = f.Address.ZipCode()
                }
            })
            .RuleFor(u => u.security, f => new SecurityInformation
            {
                UserPassword = f.Internet.Password(),
                UserPasswordHash = f.Random.AlphaNumeric(10),
                PasswordSalt = f.Random.AlphaNumeric(5),
                SecurityQuestion = "What is your favorite color?",
                SecurityAnswer = "Blue"
            })
            .RuleFor(u => u.account, f => new AccountInformation
            {
                IsEmailConfirmed = f.Random.Bool(),
                IsPhoneNumberConfirmed = f.Random.Bool(),
                TwoFactorEnabled = f.Random.Bool(),
                LockoutEnabled = f.Random.Bool(),
                LockoutEndDateUtc = f.Date.Future()
            })
            .RuleFor(u => u.metadata, f => new Metadata
            {
                LastLoginDate = f.Date.Past(),
                Roles = new List<string> { "Admin", "User" },
                Claims = new List<Claim>
                {
                    new Claim("role", "Admin"),
                    new Claim("email", f.Internet.Email())
                }
            })
            .RuleFor(u => u.social, f => new SocialInformation
            {
                ProfilePictureUrl = f.Internet.Url(),
                Bio = f.Lorem.Sentence(),
                WebsiteUrl = f.Internet.Url()
            });

        var user = faker.Generate();
        await _mongoRepository.CreateUser(user);
    }

    public async Task UpdateUser()
    {
        var faker = new Faker<User>()
        .RuleFor(u => u.UserName, f => f.Internet.UserName())
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
        .RuleFor(u => u.Gender, f => f.PickRandom(new[] { "Male", "Female" }))
        .RuleFor(u => u.contacts, f => new List<ContactInformation>
        {
            new ContactInformation
            {
                UserEmail = f.Internet.Email(),
                UserPhone = f.Phone.PhoneNumber(),
                UserPhoneNumber = f.Phone.PhoneNumber()
            }
        })
        .RuleFor(u => u.addresses, f => new List<AddressInformation>
        {
            new AddressInformation
            {
                StreetAddress = f.Address.StreetAddress(),
                City = f.Address.City(),
                State = f.Address.State(),
                Country = f.Address.Country(),
                PostalCode = f.Address.ZipCode()
            }
        })
        .RuleFor(u => u.security, f => new SecurityInformation
        {
            UserPassword = f.Internet.Password(),
            UserPasswordHash = f.Random.AlphaNumeric(10),
            PasswordSalt = f.Random.AlphaNumeric(5),
            SecurityQuestion = "What is your favorite color?",
            SecurityAnswer = "Blue"
        })
        .RuleFor(u => u.account, f => new AccountInformation
        {
            IsEmailConfirmed = f.Random.Bool(),
            IsPhoneNumberConfirmed = f.Random.Bool(),
            TwoFactorEnabled = f.Random.Bool(),
            LockoutEnabled = f.Random.Bool(),
            LockoutEndDateUtc = f.Date.Future()
        })
        .RuleFor(u => u.metadata, f => new Metadata
        {
            LastLoginDate = f.Date.Past(),
            Roles = new List<string> { "Admin", "User" },
            Claims = new List<Claim>
            {
                new Claim("role", "Admin"),
                new Claim("email", f.Internet.Email())
            }
        })
        .RuleFor(u => u.social, f => new SocialInformation
        {
            ProfilePictureUrl = f.Internet.Url(),
            Bio = f.Lorem.Sentence(),
            WebsiteUrl = f.Internet.Url()
        });

        var user = faker.Generate();
        await _mongoRepository.UpdateUser(user);
    }

    public async Task DeleteUser()
    {
        var faker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .Generate();
        await _mongoRepository.DeleteUser(faker);
    }

    public async Task GetUserById()
    {
        User user = new User();
        await _mongoRepository.GetUser(user.Id).Dump();
    }

    public async Task GetUserByName()
    {
        User user = new User();
        await _mongoRepository.GetUser(user.UserName!).Dump();
    }

    public async Task GetUserList(int size)
    {
        User user = new User();
        long countUsers = await _mongoRepository.CountOfUsers();
        int totalindex = (int)Math.Ceiling((double)countUsers / size);
        for (int index = 1; index <= totalindex; index++)
        { await _mongoRepository.GetUserList(user, index, size).Dump(); }
    }
}
