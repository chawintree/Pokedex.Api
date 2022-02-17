namespace Pokedex.Api.Domain.Models
{
    public class Result<T>
    {
        public T? Value { get; init; }
        public Status Status { get; init; }
        public bool IsSuccess { get => Status == Status.Success; } 
        public Exception? Error { get; init; }

        public Result(T? value, Status status, Exception? error = null)
        {
            Value = value;
            Status = status;
            Error = error;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, Status.Success);
        }

        public static Result<T> Failure(Exception? error = null)
        {
            return new Result<T>(default, Status.Error, error);
        }

        public static Result<T> NotFound()
        {
            return new Result<T>(default, Status.NotFound);
        }
    }
}
