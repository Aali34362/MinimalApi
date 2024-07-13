using MongoDB.Driver;

namespace MongoDBDemo.Repository;

public class MongoRepository
    (IDocumentWrapper<User> userStore) 
    : IMongoRepository
{
    private readonly IDocumentWrapper<User> _userStore = userStore ?? throw new Exception();

    #region User
    public async Task<bool> CreateUser(User user)
    {
        await _userStore.InsertOneAsync(user, "UsersCollection");
        return true;
    }
    public async Task<bool> DeleteUser(User user)
    {
        await _userStore.DeleteOneAsync(r => r.Id == user.Id, "UsersCollection");
        return true;
    }
    public async Task<bool> SoftDeleteUser(User user)
    {
        var deleteDefinition = Builders<User>.Update
            .Combine(Builders<User>.Update.Set(r => r.Actv_Ind, 0),
                 Builders<User>.Update.Set(r => r.Del_Ind, 1));
        await _userStore.UpdateOneAsync(r => r.Id == user.Id, deleteDefinition, "UsersCollection");
        return true;
    }
    public async Task<bool> UpdateUser(User user)
    {
        var updateDefinition = Builders<User>.Update
        .Combine(
            Builders<User>.Update.Set(r => r.UserName, user.UserName),
            Builders<User>.Update.Set(r => r.FirstName, user.FirstName),
            Builders<User>.Update.Set(r => r.LastName, user.LastName),
            Builders<User>.Update.Set(r => r.DateOfBirth, user.DateOfBirth),
            Builders<User>.Update.Set(r => r.Gender, user.Gender),
            Builders<User>.Update.Set(r => r.contacts, user.contacts),
            Builders<User>.Update.Set(r => r.addresses, user.addresses),
            Builders<User>.Update.Set(r => r.security, user.security),
            Builders<User>.Update.Set(r => r.account, user.account),
            Builders<User>.Update.Set(r => r.metadata, user.metadata),
            Builders<User>.Update.Set(r => r.social, user.social)
        );

        await _userStore.UpdateOneAsync(r => r.Id == user.Id, updateDefinition, "UsersCollection");
        return true;
    }

    public async Task<User> GetUser(Guid Id)
    {
        return await _userStore.FindOneAsync(r => r.Id == Id, "UsersCollection");
    }
    public async Task<User> GetUser(string UserName)
    {
        return await _userStore.FindOneAsync(r => r.UserName == UserName, "UsersCollection");
    }
    public async Task<PaginatedList<UserLists>> GetUserList(User user, int index, int size)
    {
        var filterBuilder = Builders<User>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(user.UserName))
        {
            filter &= filterBuilder.Eq(r => r.UserName, user.UserName);
        }

        var collection = _userStore.GetCollection("UsersCollection");

        long total = await _userStore.CountDocumentsAsync(filter, "UsersCollection");

        var users = await collection.Find(filter)
            .SortByDescending(r => r.Lst_Crtd_Dt)
            .Skip((index - 1) * size)
            .Limit(size)
            .ToListAsync();

        var userList = users.Select(r => new UserLists
        {
            Id = r.Id,
            DisplayName = $"{r.FirstName} {r.LastName}",
            address = ConcatenateAddresses(r.addresses),
            DateOfBirth = r.DateOfBirth,
            Gender = r.Gender,
            UserEmail = r.contacts?.FirstOrDefault()?.UserEmail!,
            UserPhone = r.contacts?.FirstOrDefault()?.UserPhone!,
            Actv_Ind = r.Actv_Ind,
            Lst_Crtd_Usr = r.Lst_Crtd_Usr,
            Lst_Crtd_Dt = r.Lst_Crtd_Dt
        }).ToList();

        return userList.ToPaginatedList(index, size, Convert.ToInt32(total));
    }
    public async Task<long> CountOfUsers() => await _userStore.CountDocumentsAsync(Builders<User>.Filter.Empty, "UsersCollection");
    public string ConcatenateAddresses(List<AddressInformation>? addresses)
    {
        if (addresses == null || !addresses.Any())
        {
            return string.Empty;
        }
        return string.Join(" | ", addresses.Select(a => $"{a.StreetAddress}, {a.City}, {a.State}, {a.Country}, {a.PostalCode}"));
    }
    #endregion

}
