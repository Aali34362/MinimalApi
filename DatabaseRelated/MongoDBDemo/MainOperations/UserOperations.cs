using Bogus;
using MongoDBDemo.JsonClass;

namespace MongoDBDemo.MainOperations;

public class UserOperations(IMongoRepository mongoRepository, IMapper mapper)
{
    private readonly IMongoRepository _mongoRepository = mongoRepository ?? throw new Exception();
    private readonly IMapper _mapper = mapper ?? throw new Exception();
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
                },
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
        User user = new();
        user.Id = Guid.Parse("9c108428-e39c-4753-a9f0-67b694dae164");
        await _mongoRepository.DeleteUser(user);
    }
    public async Task SoftDeleteUser()
    {
        User user = new();
        user.Id = Guid.Parse("74d0ea4a-7d95-4cab-82d6-0668b28e2b79");
        await _mongoRepository.SoftDeleteUser(user);
    }
    public async Task GetUserById()
    {
        User user = await _mongoRepository.GetUser(Guid.Parse("74d0ea4a-7d95-4cab-82d6-0668b28e2b79"));//// ?? throw new Exception("Data Not Found").Dump();
        if (user != null)
        {
            UserDetail userDetail = _mapper.Map<User, UserDetail>(user);
            userDetail.Dump();
        }
    }
    public async Task GetUserByName()
    {
        User user = await _mongoRepository.GetUser("Janie.Conn2");
        UserDetail userDetail = _mapper.Map<User, UserDetail>(user);
        userDetail.Dump();
    }
    public async Task GetUserList(int size = 0)
    {
        var defaultSize = size == 0 ? 5 : size;
        User user = new User();
        long countUsers = await _mongoRepository.CountOfUsers();
        int totalindex = (int)Math.Ceiling((double)countUsers / defaultSize);
        for (int index = 1; index <= totalindex; index++)
        {
            await _mongoRepository.GetUserList(user, index, defaultSize).Dump();
        }
    }
    public void DynamicJsonClass()
    {
        string json = @"
        {
          ""Crtd_Usr"": ""Admin"",
          ""Crtd_Dt"": { ""$date"": ""2024-07-13T13:58:08.143Z"" },
          ""Lst_Crtd_Usr"": ""Admin"",
          ""Lst_Crtd_Dt"": { ""$date"": ""2024-07-13T13:58:08.146Z"" },
          ""Actv_Ind"": 1,
          ""Del_Ind"": 0,
          ""UserName"": ""Hello World"",
          ""FirstName"": ""Hello"",
          ""LastName"": ""World"",
          ""DateOfBirth"": { ""$date"": ""1993-10-20T18:34:38.965Z"" },
          ""Gender"": ""Male"",
          ""contacts"": [
            {
              ""UserEmail"": ""Hello@yahoo.com"",
              ""UserPhone"": ""(582) 749-2954"",
              ""UserPhoneNumber"": ""(392) 789-3011 x7928""
            }
          ],
          ""addresses"": [
            {
              ""StreetAddress"": ""97651 Wisozk Greens"",
              ""City"": ""South Lempi"",
              ""State"": ""Kansas"",
              ""Country"": ""Nicaragua"",
              ""PostalCode"": ""55732-5141""
            }
          ],
          ""security"": {
            ""UserPassword"": ""t292qZCJSQ"",
            ""UserPasswordHash"": ""ow6nn7a8b9"",
            ""PasswordSalt"": ""6i5ye"",
            ""SecurityQuestion"": ""What is your favorite color?"",
            ""SecurityAnswer"": ""Blue""
          },
          ""account"": {
            ""IsEmailConfirmed"": true,
            ""IsPhoneNumberConfirmed"": true,
            ""TwoFactorEnabled"": false,
            ""LockoutEnabled"": true,
            ""LockoutEndDateUtc"": { ""$date"": ""2024-09-24T13:24:21.823Z"" }
          },
          ""metadata"": {
            ""LastLoginDate"": { ""$date"": ""2024-07-08T23:18:16.460Z"" },
            ""Roles"": [""Admin"", ""User""],
            ""Claims"": [
              {
                ""Issuer"": ""LOCAL AUTHORITY"",
                ""OriginalIssuer"": ""LOCAL AUTHORITY"",
                ""Subject"": null,
                ""Type"": ""role"",
                ""Value"": ""Admin"",
                ""ValueType"": ""http://www.w3.org/2001/XMLSchema#string""
              },
              {
                ""Issuer"": ""LOCAL AUTHORITY"",
                ""OriginalIssuer"": ""LOCAL AUTHORITY"",
                ""Subject"": null,
                ""Type"": ""email"",
                ""Value"": ""Keaton71@hotmail.com"",
                ""ValueType"": ""http://www.w3.org/2001/XMLSchema#string""
              }
            ]
          },
          ""social"": {
            ""ProfilePictureUrl"": ""https://braulio.biz"",
            ""Bio"": ""Laboriosam voluptas labore sed."",
            ""WebsiteUrl"": ""https://everette.com""
          }
        }";
        string className = "DynamicUser";
        string classCode = JsonClassGenerator.GenerateClass(json, className);
        Console.WriteLine(classCode);

        ////// Step 2: Compile the generated class code into a concrete class
        ////Type dynamicType = RuntimeCompiler.CompileClass(classCode, className);

        ////// Step 3: Use the generated class to insert data into MongoDB
        ////var dbHelper = new MongoDBHelper(connectionString, databaseName);
        ////var method = typeof(MongoDBHelper).GetMethod("InsertDynamicJson").MakeGenericMethod(dynamicType);
        ////method.Invoke(dbHelper, new object[] { json, collectionName });

        ////Console.WriteLine("Document inserted successfully.");
    }

}
