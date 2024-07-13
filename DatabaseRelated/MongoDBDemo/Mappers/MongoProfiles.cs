namespace MongoDBDemo.Mappers;

public class MongoProfiles : Profile
{
    public MongoProfiles()
    {
        CreateMap<User,UserDetail>();
        CreateMap<ContactInformation, ContactInformations>();
        CreateMap<AddressInformation, AddressInformations>();
        CreateMap<SecurityInformation, SecurityInformations>();
        CreateMap<AccountInformation, AccountInformations>();
        CreateMap<Metadata, Metadatas>();
        CreateMap<SocialInformation, SocialInformations>();
    }
}
