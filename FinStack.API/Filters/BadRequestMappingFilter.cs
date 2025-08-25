using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class BadRequestMappingFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BadHttpRequestException exception)
        {
            var response = new ResponseMeta
            {
                Code = exception.StatusCode,
                Message = exception.Message,
            };

            context.Result = new ObjectResult(response);
            context.ExceptionHandled = true;
        }
    }
}
