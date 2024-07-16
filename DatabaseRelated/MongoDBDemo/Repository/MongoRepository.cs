using MongoDB.Driver;
using MongoDBDemo.Models;

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
            finally
            {
                session.Dispose();
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
    public async Task<bool> CreateProduct(Product product) 
    {
        using (var session = await _productStore.StartSession())
        {
            session.StartTransaction();
            try
            {
                SequenceParam sequence = GetSequenceDetails("productcode");
                product.Product_Cd = await _productStore.GetNextSequenceValue(sequence);
                await _productStore.InsertOneAsync(product, "ProductsCollection");
                await session.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user with transaction: {ex.Message}");
                await session.AbortTransactionAsync();
                return false;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
    public async Task<bool> UpdateProduct(Product product){return true; }
    public async Task<bool> DeleteProduct(Product product)
    {
        await _productStore.DeleteOneAsync(r => r.Id == product.Id, "ProductsCollection");
        return true;
    }
    public async Task<bool> SoftDeleteProduct(Product product)
    {
        var deleteDefinition = Builders<Product>.Update
            .Combine(Builders<Product>.Update.Set(r => r.Actv_Ind, 0),
                 Builders<Product>.Update.Set(r => r.Del_Ind, 1));
        await _productStore.UpdateOneAsync(r => r.Id == product.Id, deleteDefinition, "ProductsCollection");
        return true;
    }
    public async Task<Product> GetProduct(Guid Id)
    {
        return await _productStore.FindOneAsync(r => r.Id == Id, "ProductsCollection");
    }
    public async Task<Product> GetProduct(string productName, string productCode)
    {
        var filterBuilder = Builders<Product>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(productName))
        {
            filter &= filterBuilder.Eq(r => r.Product_Nm, productName);
        }

        if (!string.IsNullOrEmpty(productCode))
        {
            filter &= filterBuilder.Eq(r => r.Product_Cd, productCode);
        }

        return await _productStore.FindOneAsync(filter, "ProductsCollection");
    }
    public async Task<PaginatedList<ProductList>> GetProductList(Product product, int index, int size)
    {
        var filterBuilder = Builders<Product>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(product.Product_Nm))
        {
            filter &= filterBuilder.Eq(r => r.Product_Nm, product.Product_Nm);
        }
        if (!string.IsNullOrEmpty(product.Product_Cd))
        {
            filter &= filterBuilder.Eq(r => r.Product_Cd, product.Product_Cd);
        }

        var collection = _productStore.GetCollection("ProductsCollection");

        long total = await _productStore.CountDocumentsAsync(filter, "ProductsCollection");

        var users = await collection.Find(filter)
            .SortByDescending(r => r.Lst_Crtd_Dt)
            .Skip((index - 1) * size)
            .Limit(size)
            .ToListAsync();

        var userList = users.Select(r => new ProductList
        {
            Id = r.Id,
            Product_Nm = r.Product_Nm,
            Product_Cd = r.Product_Cd,
            Actv_Ind = r.Actv_Ind,
            Lst_Crtd_Usr = r.Lst_Crtd_Usr,
            Lst_Crtd_Dt = r.Lst_Crtd_Dt
        }).ToList();

        return userList.ToPaginatedList(index, size, Convert.ToInt32(total));
    }
    public async Task<long> CountOfProducts() => await _productStore.CountDocumentsAsync(Builders<Product>.Filter.Empty, "ProductsCollection");
    #endregion

    #region Companies
    public async Task<bool> CreateCompany(Company company)
    {
        using (var session = await _companyStore.StartSession())
        {
            session.StartTransaction();
            try
            {
                SequenceParam sequence = GetSequenceDetails("companycode");
                company.Company_Cd = await _companyStore.GetNextSequenceValue(sequence);
                await _companyStore.InsertOneAsync(company, "CompaniesCollection");
                await session.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating company with transaction: {ex.Message}");
                await session.AbortTransactionAsync();
                return false;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
    public async Task<bool> UpdateCompany(Company company) { return true; }
    public async Task<bool> DeleteCompany(Company company)
    {
        await _companyStore.DeleteOneAsync(r => r.Id == company.Id, "CompaniesCollection");
        return true;
    }
    public async Task<bool> SoftDeleteCompany(Company company)
    {
        var deleteDefinition = Builders<Company>.Update
            .Combine(Builders<Company>.Update.Set(r => r.Actv_Ind, 0),
                 Builders<Company>.Update.Set(r => r.Del_Ind, 1));
        await _companyStore.UpdateOneAsync(r => r.Id == company.Id, deleteDefinition, "CompaniesCollection");
        return true;
    }
    public async Task<Company> GetCompany(Guid Id)
    {
        return await _companyStore.FindOneAsync(r => r.Id == Id, "CompaniesCollection");
    }
    public async Task<Company> GetCompany(string companyName, string companyCode)
    {
        var filterBuilder = Builders<Company>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(companyName))
        {
            filter &= filterBuilder.Eq(r => r.Company_Nm, companyName);
        }

        if (!string.IsNullOrEmpty(companyCode))
        {
            filter &= filterBuilder.Eq(r => r.Company_Cd, companyCode);
        }

        return await _companyStore.FindOneAsync(filter, "CompaniesCollection");
    }
    public async Task<PaginatedList<CompanyList>> GetProductList(Company company, int index, int size)
    {
        var filterBuilder = Builders<Company>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(company.Company_Nm))
        {
            filter &= filterBuilder.Eq(r => r.Company_Nm, company.Company_Nm);
        }
        if (!string.IsNullOrEmpty(company.Company_Cd))
        {
            filter &= filterBuilder.Eq(r => r.Company_Cd, company.Company_Cd);
        }

        var collection = _companyStore.GetCollection("CompaniesCollection");

        long total = await _companyStore.CountDocumentsAsync(filter, "CompaniesCollection");

        var companies = await collection.Find(filter)
            .SortByDescending(r => r.Lst_Crtd_Dt)
            .Skip((index - 1) * size)
            .Limit(size)
            .ToListAsync();

        var companiesList = companies.Select(r => new CompanyList
        {
            Id = r.Id,
            Company_Cd = r.Company_Cd,
            Company_Nm = r.Company_Nm,
            Actv_Ind = r.Actv_Ind,
            Lst_Crtd_Usr = r.Lst_Crtd_Usr,
            Lst_Crtd_Dt = r.Lst_Crtd_Dt
        }).ToList();

        return companiesList.ToPaginatedList(index, size, Convert.ToInt32(total));
    }
    public async Task<long> CountOfCompanies() => await _companyStore.CountDocumentsAsync(Builders<Company>.Filter.Empty, "CompaniesCollection");
    #endregion

    private SequenceParam GetSequenceDetails(string sequenceName)
    {
        SequenceParam sequence = new();
        if(sequenceName is "productcode")
        {
            sequence.sequenceName = "productcode";
            sequence.prefix = "PC";
            sequence.startWith = 1;
            sequence.incrementBy = 1;
            sequence.initialPadding = 4;
        }
        if (sequenceName is "companycode")
        {
            sequence.sequenceName = "companycode";
            sequence.prefix = "CC";
            sequence.startWith = 100;
            sequence.incrementBy = 1;
            sequence.initialPadding = 3;
        }
        return sequence;
    }


}


public class RedisJson(IMongoDatabase database,IMapper mapper)
{
    private readonly IMongoCollection<Company> _companyCollection = database.GetCollection<Company>("CompaniesCollection");
    private readonly IMongoCollection<Product> _productCollection = database.GetCollection<Product>("ProductsCollection");
    private readonly IMongoCollection<User> _userCollection = database.GetCollection<User>("UsersCollection");
    private readonly IMapper _mapper = mapper;
    #region CombineAllJson
    public async Task<FinalOutputDetails> GetCompanyDetailsAsync(string companyCode)
    {
        FinalOutputDetails finalOutput = new();
        List<CompanyProductDetails> companyProducts = new();
        List<ProductUserDetails> productUsers = new();
        var company = await _companyCollection.Find(c => c.Company_Cd == companyCode).FirstOrDefaultAsync();
        if (company != null)
        {
            finalOutput = _mapper.Map<Company, FinalOutputDetails>(company);

            var productCodes = finalOutput.companyProducts.Select(p => p.Product_Cd).ToList();
            var products = await _productCollection.Find(p => productCodes.Contains(p.Product_Cd)).ToListAsync();

            companyProducts = _mapper.Map<List<Product>, List<CompanyProductDetails>>(products);

            foreach (var product in companyProducts)
            {
                var userIds = product.productUsers.Select(u => u.Id).ToList();
                var users = await _userCollection.Find(u => userIds.Contains(u.Id.ToString())).ToListAsync();
                productUsers = _mapper.Map<List<User>, List<ProductUserDetails>>(users);
                product.productUsers = productUsers;
            }
            finalOutput.companyProducts = companyProducts;
        }
        return finalOutput!;
    }

    public async Task<FinalOutputDetails> GetCompanyDetailsAsync2(string companyCode)
    {
        FinalOutputDetails finalOutput = new();
        var company = await _companyCollection.Find(c => c.Company_Cd == companyCode).FirstOrDefaultAsync();
        if (company != null)
        {
            finalOutput = _mapper.Map<Company, FinalOutputDetails>(company);

            var productCodes = company.companyProducts?.Select(p => p.Product_Cd).ToList();
            var products = await _productCollection.Find(p => productCodes!.Contains(p.Product_Cd)).ToListAsync();

            var allUserIds = products.SelectMany(p => p.productUsers!).Select(u => u.UserId).Distinct().ToList();
            var users = await _userCollection.Find(u => allUserIds.Contains(u.Id)).ToListAsync();
            var userDictionary = users.ToDictionary(u => u.Id, u => _mapper.Map<User, ProductUserDetails>(u));

            var companyProducts = products.Select(product =>
            {
                var productDetails = _mapper.Map<Product, CompanyProductDetails>(product);
                productDetails.productUsers = product.productUsers?
                    .Select(u => userDictionary[u.UserId])
                    .ToList();
                return productDetails;
            }).ToList();

            finalOutput.companyProducts = companyProducts;
        }
        return finalOutput!;
    }
    #endregion
}