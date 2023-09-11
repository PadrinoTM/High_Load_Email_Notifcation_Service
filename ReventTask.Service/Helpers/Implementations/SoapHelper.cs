namespace ReventTask.Core.Services;
public class SoapHelper : ISoapHelper
{
    private readonly IHttpClientFactory _client;
    private readonly ILogger _logger;

    public SoapHelper(IHttpClientFactory client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<Result<T>> ConsumeApi<T>(string payload, string url, string serviceProvider, Dictionary<string, string> headers = null, bool logRequest = true, bool logResponse = true)
    {
        var apiResult = new Result<T>();

        var outboundLog = new OutboundLog
        {
            EndPointCalled = url,
            LogDate = DateTime.UtcNow,
            RequestDateTime = DateTime.UtcNow,
            ServiceProvider = serviceProvider,
            RequestDetails = "Not Logged",
            ResponseDetails = "Not Logged"
        };

        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            if (logRequest)
            {
                outboundLog.RequestDetails = payload;
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    requestMessage.Headers.Add(item.Key, item.Value);
                }
            }

            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "text/xml");

            var client = _client.CreateClient();

            var response = await client.SendAsync(requestMessage);


            outboundLog.ResponseDateTime = DateTime.UtcNow;
            var result = await response.Content.ReadAsStringAsync();

            if (logResponse)
            {
                outboundLog.ResponseDetails = result;
            }

            if (response.IsSuccessStatusCode)
            {
                apiResult.Content = DeserializeXml<T>(result);
                apiResult.Message = result;
                outboundLog.IsSuccess = true;
                apiResult.OutboundLog = outboundLog;
            }
            outboundLog.ErrorMessage = response.StatusCode.ToString();
            outboundLog.IsSuccess = false;
            apiResult.OutboundLog = outboundLog;
            return apiResult;
        }
        catch (Exception ex)
        {
            apiResult.Content = (T)(object)null;
            outboundLog.ErrorMessage = ex.Message;
            outboundLog.IsSuccess = false;
            return apiResult;
        }
    }

    public T DeserializeXml<T>(string xmlString)
    {

        xmlString = xmlString.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("&lt;", "<").Replace("&gt;", ">").Replace("< ", "<").Replace(" >", ">").Replace(" />", "/>").Replace("</ ", "</");

        var xDoc = new XmlDocument();
        xDoc.LoadXml(xmlString);
        var xNodeReader = new XmlNodeReader(xDoc.DocumentElement);
        var serializer = new XmlSerializer(typeof(T));

        var result = serializer.Deserialize(xNodeReader);

        return (T)result;
    }
}