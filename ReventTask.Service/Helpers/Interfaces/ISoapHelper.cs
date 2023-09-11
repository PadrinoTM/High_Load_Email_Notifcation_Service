namespace ReventTask.Core.Services;
public interface ISoapHelper
{
    Task<Result<T>> ConsumeApi<T>(string payload, string url, string serviceProvider, Dictionary<string, string> headers = null, bool logRequest = true, bool logResponse = true);
    T DeserializeXml<T>(string xmlString);
}