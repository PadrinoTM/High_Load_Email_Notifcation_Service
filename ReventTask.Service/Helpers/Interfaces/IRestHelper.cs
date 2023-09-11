namespace ReventTask.Core.Services;
public interface IRestHelper
{
    Task<Result<TResponse>> ConsumeApi<TResponse>(string httpNamedClient, string url, object payload, string serviceProvider, ApiType type = ApiType.Get, Dictionary<string, string> headers = null, bool logRequest = true, bool logResponse = true);
}