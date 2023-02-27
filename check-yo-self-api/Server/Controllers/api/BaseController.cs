using check_yo_self_api.Server.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace check_yo_self_api.Server.Controllers.api;

[Authorize]
[ApiController]
[ServiceFilter(typeof(ApiExceptionFilter))]
[Route("api/v{version:apiVersion}/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public class BaseController : Controller
{
    public BaseController()
    {
    }
}
