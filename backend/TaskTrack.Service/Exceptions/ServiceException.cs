namespace TaskTrack.Service.Exceptions;

public class ServiceException(string message, int statusCode = 400, IDictionary<string, string[]>? errors = null) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public IDictionary<string, string[]> Errors { get; } = errors ?? new Dictionary<string, string[]>();
}
