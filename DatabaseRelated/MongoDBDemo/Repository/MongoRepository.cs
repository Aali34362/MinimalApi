using MongoDB.Driver;

namespace MongoDBDemo.Repository;

public class MongoRepository
    (IDocumentWrapper<User> userStore, IDocumentWrapper<Product> productStore, IDocumentWrapper<Company> companyStore) 
    : IMongoRepository
{
    private readonly IDocumentWrapper<User> _userStore = userStore ?? throw new Exception();
    private readonly IDocumentWrapper<Product> _productStore = productStore ?? throw new Exception();
    private readonly IDocumentWrapper<Company> _companyStore = companyStore ?? throw new Exception();

    #region User
    public async Task<bool> CreateUser(User user)
    {
        using (var session = await _userStore.StartSession())
        {
            session.StartTransaction();
            try
            {
                await _userStore.InsertOneAsync(user, "UsersCollection");
                await session.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user with transaction: {ex.Message}");
                await session.AbortTransactionAsync();
                return false;
            }
        }
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

    #region Products
    public async Task<bool> CreateProduct(Product product) { return true; }
    public async Task<bool> UpdateProduct(Product product){return true; }
    public async Task<bool> DeleteProduct(Product product){return true; }
    public async Task<bool> SoftDeleteProduct(Product product){return true; }
    public async Task<Product> GetProduct(Guid Id){return null;}
    public async Task<Product> GetProduct(string productName, string productCode){return null;}
    public async Task<PaginatedList<ProductList>> GetProductList(Product product, int index, int size){return null;}
    public async Task<long> CountOfProducts() { return 0; }
    #endregion

    #region Companies
    public async Task<bool> CreateCompany(Company company){return true; }
    public async Task<bool> UpdateCompany(Company company){return true; }
    public async Task<bool> DeleteCompany(Company company){return true; }
    public async Task<bool> SoftDeleteCompany(Company company){return true; }
    public async Task<Company> GetCompany(Guid Id){return null;}
    public async Task<Company> GetCompany(string companyName, string companyCode){return null;}
    public async Task<PaginatedList<CompanyList>> GetProductList(Company company, int index, int size){return null;}
    public async Task<long> CountOfCompanies() { return 0; }
    #endregion
}
