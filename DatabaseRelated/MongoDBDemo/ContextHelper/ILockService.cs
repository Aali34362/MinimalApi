namespace MongoDBDemo.ContextHelper;

public interface ILockService
{
    Task<bool> AcquireLockAsync(Lock lockEntry);
    Task ReleaseLockAsync(string resource);
    Task<bool> IsLockedAsync(string resource, string requestedOperation);
}
