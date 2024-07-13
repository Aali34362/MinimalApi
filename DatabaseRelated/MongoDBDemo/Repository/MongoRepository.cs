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

    public async Task<PaginatedList<User>> GetUserList(User user)
    {
        PaginatedList<User> list = new PaginatedList<User>();
        return list;
    }
    #endregion

}
