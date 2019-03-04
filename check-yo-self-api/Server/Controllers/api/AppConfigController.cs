using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using check_yo_self_api.Server.Entities.Config;
using NSwag.Annotations;

namespace check_yo_self_api.Server.Controllers.api
{
    [Route("api/[controller]")]
    [SwaggerIgnore]
    [AllowAnonymous]
    public class AppConfigController : BaseController
    {
        private readonly ILogger _logger;
        private readonly AppConfig _appConfig;

        public AppConfigController(IOptionsSnapshot<AppConfig> appConfig, ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger<AppConfigController>();
            _appConfig = appConfig.Value;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            return Ok(_appConfig);
        }
    }
}