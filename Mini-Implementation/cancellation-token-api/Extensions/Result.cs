namespace cancellation_token_api.Extensions;

//public class Result<T>
//{
//    public T Value { get; }
//    public bool IsSuccess { get; }
//    public string Error { get; }
//    protected Result(bool isSuccess, string error)
//    {
//        IsSuccess = isSuccess;
//        Error = error;
//    }
//    public Result(T value)
//    {
//        Value = value;
//        IsSuccess = true;
//    }

//    public static Result Success() => new Result(true, null);
//    public static Result Failure(string error) => new Result(false, error);

//    public T Match<T>(Func<T> success, Func<string, T> failure)
//        => IsSuccess ? success() : failure(Error);
//    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onError)
//        => IsSuccess ? onSuccess(Value) : onError(ErrorMessage);
//}
