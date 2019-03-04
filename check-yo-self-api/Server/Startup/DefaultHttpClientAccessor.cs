using System.Net.Http;

namespace check_yo_self_api.Server.Startup
{
  public class DefaultHttpClientAccessor : IHttpClientAccessor
  {
    public HttpClient Client { get; }

    public DefaultHttpClientAccessor()
    {
      Client = new HttpClient();
    }
  }
}