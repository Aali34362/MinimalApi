namespace MongoDBDemo.ContextHelper;

public class LockService(IDocumentWrapper<Lock> lockCollection) : ILockService
{
    private readonly IDocumentWrapper<Lock> _lockCollection = lockCollection ?? throw new Exception();

    public async Task<bool> AcquireLockAsync(Lock lockEntry)
    {
        try
        {
            await _lockCollection.InsertOneAsync(lockEntry, "LocksCollection");
            return true;
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            // Lock already exists
            return false;
        }
    }

    public async Task ReleaseLockAsync(string resource)
    {
        await _lockCollection.DeleteOneAsync(r => r.Resource == resource, "LocksCollection");
    }

    public async Task<bool> IsLockedAsync(string resource, string requestedOperation)
    {
        var existingLock = await _lockCollection.FindOneAsync(r => r.Resource == resource, "LocksCollection").Dump();
        if (existingLock == null)// No existing lock
            return false;

        if (existingLock.Operation == "write" || requestedOperation == "write")// Any write operation conflicts with existing locks
            return true;
        
        return false;// Both operations are read, no conflict
    }
}
