using check_yo_self_api.Server.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace check_yo_self_api.Server.Filters;

public class ModelValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.ModelState.IsValid)
        {
            if (filterContext.HttpContext.Request.Method == "GET")
            {
                var result = new BadRequestResult();
                filterContext.Result = result;
            }
            else
            {
                var result = new ContentResult();
                var content = JsonConvert.SerializeObject(filterContext.ModelState.GetModelErrors(),
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                result.Content = content;
                result.ContentType = "application/json";

                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.Result = result;
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext filterContext)
    {

    }

}
