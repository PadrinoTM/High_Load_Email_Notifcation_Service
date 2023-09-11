namespace ReventTask.Core.Services;
public class RestHelper : IRestHelper
{
    private readonly IHttpClientFactory _client;
    private readonly ILogger _logger;

    public RestHelper(IHttpClientFactory client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }


    public async Task<Result<TResponse>> ConsumeApi<TResponse>(string httpNamedClient, string url, object payload, string serviceProvider,
                    ApiType type = ApiType.Post, Dictionary<string, string> headers = null, bool logRequest = true, bool logResponse = true)
    {
        var apiResult = new Result<TResponse>();
        var outboundLog = new OutboundLog
        {
            EndPointCalled = url,
            LogDate = DateTime.UtcNow,
            RequestDateTime = DateTime.UtcNow,
            ServiceProvider = serviceProvider,
            RequestDetails = "Not Logged",
            ResponseDetails = "Not Logged",
        };
        try
        {
            var client = _client.CreateClient(httpNamedClient);
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = type switch
                {
                    ApiType.Post => HttpMethod.Post,
                    ApiType.Put => HttpMethod.Put,
                    ApiType.Patch => HttpMethod.Patch,
                    ApiType.Delete => HttpMethod.Delete,
                    _ => HttpMethod.Get,
                }
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    requestMessage.Headers.Add(item.Key, item.Value);
                }
            }

            var serializePayload = JsonConvert.SerializeObject(payload);

            if (logRequest)
            {
                outboundLog.RequestDetails = serializePayload;
            }


            if (payload != null)
            {
                requestMessage.Content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response;
            if (type == ApiType.Get && headers == null)
            {

                response = await client.GetAsync(url);
            }
            else if (type == ApiType.Post && headers == null)
            {
                response = await client.PostAsync(url, requestMessage.Content);
            }
            else
            {
                response = await client.SendAsync(requestMessage);
            }

            outboundLog.ResponseDateTime = DateTime.UtcNow;
            var result = await response.Content.ReadAsStringAsync();
            if (logResponse)
            {
                outboundLog.ResponseDetails = result;
            }


            if (response.IsSuccessStatusCode)
            {
                apiResult.Content = JsonConvert.DeserializeObject<TResponse>(result);
                apiResult.Message = "Successful";
                outboundLog.IsSuccess = true;
                apiResult.OutboundLog = outboundLog;
                outboundLog.ErrorMessage = "";
                return apiResult;
            }
            outboundLog.ErrorMessage = response.StatusCode.ToString();
            outboundLog.IsSuccess = false;
            apiResult.OutboundLog = outboundLog;

            return apiResult;
        }
        catch (Exception ex)
        {
            apiResult.Content = (TResponse)(object)null;
            outboundLog.ErrorMessage = ex.Message;
            outboundLog.IsSuccess = false;
            apiResult.IsSuccess = false;
            outboundLog.ResponseDateTime = DateTime.UtcNow;
            apiResult.OutboundLog = outboundLog;
            return apiResult;

        }
    }

}