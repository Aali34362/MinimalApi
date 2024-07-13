using MongoDB.Driver;
using System.Linq.Expressions;

namespace MongoDBDemo.ContextHelper;

public class DocumentWrapper<T> : IDocumentWrapper<T>
{
    private readonly IMongoDatabase _database;
    private readonly MongoClient _client;
    public DocumentWrapper(string connectionString, string databaseName)
    {
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(databaseName);
    }
    public async Task InsertOneAsync(T document, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(document);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting document: {ex.Message}");
            throw;
        }
    }
    public async Task InsertOneAsync(IClientSessionHandle session, T document, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(session, document);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting document: {ex.Message}");
            throw;
        }
    }
    public async Task InsertManyAsync(IEnumerable<T> documents, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.InsertManyAsync(documents);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting documents: {ex.Message}");
            throw;
        }
    }
    public async Task InsertManyAsync(IClientSessionHandle session, IEnumerable<T> documents, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.InsertManyAsync(session, documents);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting documents: {ex.Message}");
            throw;
        }
    }
    public async Task UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating document: {ex.Message}");
            throw;
        }
    }
    public async Task UpdateOneAsync(IClientSessionHandle session, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.UpdateOneAsync(session, filter, update);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating document: {ex.Message}");
            throw;
        }
    }
    public async Task UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.UpdateManyAsync(filter, update);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating documents: {ex.Message}");
            throw;
        }
    }
    public async Task UpdateManyAsync(IClientSessionHandle session, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.UpdateManyAsync(session, filter, update);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating documents: {ex.Message}");
            throw;
        }
    }
    public async Task ReplaceOneAsync(Expression<Func<T, bool>> filter, T replacement, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.ReplaceOneAsync(filter, replacement);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error replacing document: {ex.Message}");
            throw;
        }
    }
    public async Task ReplaceOneAsync(IClientSessionHandle session, Expression<Func<T, bool>> filter, T replacement, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.ReplaceOneAsync(session, filter, replacement);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error replacing document: {ex.Message}");
            throw;
        }
    }
    public Task ReplaceManyAsync(Expression<Func<T, bool>> filter, T replacement, string collectionName)
    {
        throw new NotImplementedException();
    }
    public async Task DeleteOneAsync(Expression<Func<T, bool>> filter, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting document: {ex.Message}");
            throw;
        }
    }
    public async Task DeleteOneAsync(IClientSessionHandle session, Expression<Func<T, bool>> filter, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.DeleteOneAsync(session, filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting document: {ex.Message}");
            throw;
        }
    }
    public async Task DeleteManyAsync(Expression<Func<T, bool>> filter, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.DeleteManyAsync(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting documents: {ex.Message}");
            throw;
        }
    }
    public async Task DeleteManyAsync(IClientSessionHandle session, Expression<Func<T, bool>> filter, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.DeleteManyAsync(session, filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting documents: {ex.Message}");
            throw;
        }
    }
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            var cursor = await collection.FindAsync(filter);
            return await cursor.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding documents: {ex.Message}");
            throw;
        }
    }
    public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding document: {ex.Message}");
            throw;
        }
    }
    public async Task<long> CountDocumentsAsync(FilterDefinition<T> filter, string collectionName)
    {
        try
        {
            var collection = _database.GetCollection<T>(collectionName);
            return await collection.CountDocumentsAsync(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error counting documents: {ex.Message}");
            throw;
        }
    }
    public IMongoCollection<T> GetCollection(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
    public IClientSessionHandle StartSession()
    {
        return _client.StartSession();
    }

    
}
