using capybara_api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace capybara_api.Attributes;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter {
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context) {
        if(context.Exception is HttpResponseException httpResponseException) {
            context.Result = new ObjectResult(new {
                error = httpResponseException.value
            }) {
                StatusCode = httpResponseException.statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}