using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace check_yo_self_api_client.Configuration;

public class SdkConfiguration
{
    public string BaseUrl { get; set; }
    public ILoggerFactory LoggerFactory { get; set; }
    public IHttpClientFactory HttpClientFactory { get; set; }
    // public string AuthTokenAuthority { get; set; }
    // public string AuthTokenClientId { get; set; }
    // public string AuthTokenClientSecret { get; set; }
    // public IEnumerable<string> AuthTokenScopes { get; set; }
    // public bool AllowAuthTokenHttpAuthority { get; set; }
}
