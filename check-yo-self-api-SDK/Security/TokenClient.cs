using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using check_yo_self_api_client.Configuration;
using Microsoft.Extensions.Logging;

namespace check_yo_self_api_client.Security;

public abstract class TokenClient
{
    public abstract string ClientName { get; }
    public string BaseUrl { get; private set; }
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    // private readonly string _authTokenAuthority;
    // private readonly string _authTokenClientId;
    // private readonly string _authTokenClientSecret;
    // private readonly string _authTokenScopes;
    // private readonly bool _allowAuthTokenHttpAuthority;

    public TokenClient(SdkConfiguration config)
    {
        BaseUrl = config.BaseUrl;
        _logger = config.LoggerFactory.CreateLogger(GetType());
        _httpClient = config.HttpClientFactory.CreateClient(ClientName);

        // _authTokenAuthority = config.AuthTokenAuthority;
        // _authTokenClientId = config.AuthTokenClientId;
        // _authTokenClientSecret = config.AuthTokenClientSecret;
        // _authTokenScopes = string.Join(" ", config.AuthTokenScopes);
        // _allowAuthTokenHttpAuthority = config.AllowAuthTokenHttpAuthority;
    }

#pragma warning disable IDE0060 
    protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create HttpRequestMessage");
        var request = new HttpRequestMessage();
        // var token = await GetTokenAsync(cancellationToken);

        // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        return request;
    }

    protected Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken) => Task.FromResult(_httpClient);
#pragma warning restore IDE0060
}
