using System.Collections.Generic;
using System.Linq;
using static Microsoft.AspNetCore.ResponseCompression.ResponseCompressionDefaults;

namespace check_yo_self_api.Server.Startup;

public static class DefaultMimeTypes
{
    public static IEnumerable<string> Get => MimeTypes.Concat(new[]
    {
        "image/svg+xml",
        "application/font-woff2"
    });
}
