namespace MongoDBDemo.Repository;

public interface IMongoRepository
{
    #region Users
    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(User user);
    Task<User> GetUser(Guid Id);
    Task<User> GetUser(string UserName);
    Task<PaginatedList<User>> GetUserList(User user);
    #endregion
}
