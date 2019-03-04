using System;
using System.Linq;
using System.Threading.Tasks;
using check_yo_self_api.Server.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using check_yo_self_api.Server.Startup;

namespace check_yo_self_api.Server.Controllers.api
{
    [Route("api/[controller]")]
    //[Authorize]
    [AllowAnonymous]
    public class GettingStartedController : BaseController
    {
        //private readonly ApplicationDbContext _context;
        //private readonly HttpClient _client;

        private readonly ILogger _logger;
        
        public GettingStartedController(/*ApplicationDbContext context, IHttpClientAccessor httpClientAccessor,*/ ILoggerFactory loggerFactory)
        {
            //_context = context;
            //_client = httpClientAccessor.Client;
            _logger = loggerFactory.CreateLogger<GettingStartedController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(GettingStarted), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var gettingStarted = new GettingStarted();
                gettingStarted.ApplicationName = "check_yo_self_api";
                return await Task.FromResult(Ok(gettingStarted));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Unable to get status");
                return BadRequest();
            }

        }

    }

}
