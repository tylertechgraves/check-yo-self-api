using System.Net.Http;
namespace check_yo_self_api.Server.Startup
{
  public interface IHttpClientAccessor
  {
    HttpClient Client { get; }
  }
}