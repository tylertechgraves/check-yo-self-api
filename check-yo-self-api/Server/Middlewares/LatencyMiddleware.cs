using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace check_yo_self_api.Server;

public static class IApplicationBuilderExtensions
{
    // Usage (use only for dev purposes to simulate the request delays etc)
    // app.UseSimulatedLatency(
    //     min: TimeSpan.FromMilliseconds(200),
    //     max: TimeSpan.FromMilliseconds(400));

    public static IApplicationBuilder UseSimulatedLatency(this IApplicationBuilder app, TimeSpan min, TimeSpan max)
    {
        var random = new Random();

        return app.Use(async (context, next) =>
        {
            var minDelayInMs = (int)min.TotalMilliseconds;
            var maxDelayInMs = (int)max.TotalMilliseconds;

            var delayInMs = random.Next(minDelayInMs, maxDelayInMs);

            await Task.Delay(delayInMs);
            await next();
        });
    }
}
