using MongoDB.Driver;
using System.Linq.Expressions;

namespace MongoDBDemo.ContextHelper;

public interface IDocumentWrapper<T>
{
    Task InsertOneAsync(T document, string collectionName);
    Task InsertManyAsync(IEnumerable<T> documents, string collectionName);
    Task UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, string collectionName);
    Task UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, string collectionName);
    Task ReplaceOneAsync(Expression<Func<T, bool>> filter, T replacement, string collectionName);
    Task ReplaceManyAsync(Expression<Func<T, bool>> filter, T replacement, string collectionName);
    Task DeleteOneAsync(Expression<Func<T, bool>> filter, string collectionName);
    Task DeleteManyAsync(Expression<Func<T, bool>> filter, string collectionName);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, string collectionName);
    Task<T> FindOneAsync(Expression<Func<T, bool>> filter, string collectionName);
    Task<T> FindOneAsync(FilterDefinition<T> filter, string collectionName);
    Task<long> CountDocumentsAsync(FilterDefinition<T> filter, string collectionName);
    IMongoCollection<T> GetCollection(string collectionName);
    Task<IClientSessionHandle> StartSession();
    Task<string> GetNextSequenceValue(SequenceParam sequenceParam);
    Task<string> GetNextSequenceValue2(SequenceParam sequenceParam);
    void InsertDynamicJson<J>(string json, string collectionName);
}
