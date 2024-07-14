using MongoDBDemo.Responses;

namespace MongoDBDemo.Repository;

public interface IMongoRepository
{
    #region Users
    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(User user);
    Task<bool> SoftDeleteUser(User user);
    Task<User> GetUser(Guid Id);
    Task<User> GetUser(string UserName);
    Task<PaginatedList<UserLists>> GetUserList(User user, int index, int size);
    Task<long> CountOfUsers();
    #endregion

    #region Products
    Task<bool> CreateProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(Product product);
    Task<bool> SoftDeleteProduct(Product product);
    Task<Product> GetProduct(Guid Id);
    Task<Product> GetProduct(string productName,string productCode);
    Task<PaginatedList<ProductList>> GetProductList(Product product, int index, int size);
    Task<long> CountOfProducts();
    #endregion

    #region Companies
    Task<bool> CreateCompany(Company company);
    Task<bool> UpdateCompany(Company company);
    Task<bool> DeleteCompany(Company company);
    Task<bool> SoftDeleteCompany(Company company);
    Task<Company> GetCompany(Guid Id);
    Task<Company> GetCompany(string companyName,string companyCode);
    Task<PaginatedList<CompanyList>> GetProductList(Company company, int index, int size);
    Task<long> CountOfCompanies();
    #endregion

}
