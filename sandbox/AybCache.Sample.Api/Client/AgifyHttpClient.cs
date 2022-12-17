
namespace AybCache.Sample.Api.Client;

public class AgifyHttpClient : IAgifyHttpClient
{
    private readonly HttpClient _httpClient;

    public AgifyHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [AybCache(Seconds = 600, CacheKey = CacheKeys.Client.AgifyWithName)]
    public async Task<Agify> GetAgify(string name, CancellationToken cancellationToken = default)
    {
        Thread.Sleep(2000);
        
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.agify.io/?name=" + name);
        var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            var agify = await JsonSerializer.DeserializeAsync<Agify>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, cancellationToken);
            
            
            return agify;
        }
        
        throw new HttpRequestException("God damn");
    }
}