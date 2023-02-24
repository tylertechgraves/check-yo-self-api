using check_yo_self_api_client.Configuration;
using check_yo_self_api_client.Security;

namespace check_yo_self_api_client;

public class ClientBase : TokenClient
{
    public override string ClientName => "check-yo-self-api-sdk";

    public ClientBase(SdkConfiguration config) : base(config) { }
}
