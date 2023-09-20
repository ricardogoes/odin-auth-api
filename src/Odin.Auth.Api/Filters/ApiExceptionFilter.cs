using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Infra.Keycloak.Exceptions;

namespace Odin.Auth.Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _environment;

        public ApiExceptionFilter(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            var details = new ProblemDetails();
            var exception = context.Exception;

            if (_environment.IsDevelopment())
                details.Extensions.Add("StackTrace", exception.StackTrace);

            if (exception is EntityValidationException)
            {
                details.Title = "Unprocessable entity";
                details.Status = StatusCodes.Status422UnprocessableEntity;
                details.Type = "UnprocessableEntity";
                details.Detail = exception.Message;

                details.Extensions["errors"] = (exception as EntityValidationException)?.Errors;
            }
            else if (exception is NotFoundException)
            {
                details.Title = "Not found";
                details.Status = StatusCodes.Status404NotFound;
                details.Type = "NotFound";
                details.Detail = exception.Message;
            }
            else if (exception is BadRequestException)
            {
                details.Title = "Bad request";
                details.Status = StatusCodes.Status400BadRequest;
                details.Type = "BadRequest";
                details.Detail = exception!.Message;
            }
            else if (exception is KeycloakException)
            {
                details.Title = "Invalid Authentication";
                details.Status = StatusCodes.Status401Unauthorized;
                details.Type = "InvalidAuth";
                details.Detail = exception!.Message;
            }
            else
            {
                details.Title = "An unexpected error ocurred";
                details.Status = StatusCodes.Status500InternalServerError;
                details.Type = "InternalServerError";
                details.Detail = exception.Message;
            }

            context.HttpContext.Response.StatusCode = details.Status.Value;
            context.Result = new ObjectResult(details);
            context.ExceptionHandled = true;
        }
    }
}
