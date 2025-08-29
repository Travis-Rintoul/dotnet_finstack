using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinStack.API.Filters;

public class BadRequestMappingFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BadHttpRequestException exception)
        {
            var response = new ResponseMeta
            {
                Message = exception.Message,
            };

            context.Result = new ObjectResult(response);
            context.ExceptionHandled = true;
        }
    }
}
