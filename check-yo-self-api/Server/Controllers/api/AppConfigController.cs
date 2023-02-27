using check_yo_self_api.Server.Entities.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSwag.Annotations;

namespace check_yo_self_api.Server.Controllers.api;

[Route("api/[controller]")]
[OpenApiIgnore]
[AllowAnonymous]
public class AppConfigController : BaseController
{
    private readonly AppConfig _appConfig;

    public AppConfigController(IOptionsSnapshot<AppConfig> appConfig)
    {
        _appConfig = appConfig.Value;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_appConfig);
    }
}
