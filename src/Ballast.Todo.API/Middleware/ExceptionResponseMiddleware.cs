using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Text;
using Ballast.Todo.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ballast.Todo.API.Middleware
{
    /// <summary>
    /// Middleware to handle all application exceptions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExceptionResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionResponseMiddleware> _logger;

        /// <summary>
        /// Constructor receving a <see cref="RequestDelegate"/>
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/></param>
        public ExceptionResponseMiddleware(RequestDelegate next, ILogger<ExceptionResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="context">The current <see cref="HttpContext"/></param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            ProblemDetails problemDetails;

            // BadHttpRequestException thrown by the framework
            if (exception is BadHttpRequestException bhre)
            {
                problemDetails = new ValidationProblemDetails
                {
                    Title = "Invalid request",
                    Status = (int?)typeof(BadHttpRequestException).GetProperty("StatusCode", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(bhre),
                };
            }
            // Custom BadRequestException
            else if (exception is BadRequestException bre)
            {
                var errors = new Dictionary<string, string[]>(bre.Errors.Select(error => new KeyValuePair<string, string[]>(error.Key, [.. error.Messages])));
                problemDetails = new ValidationProblemDetails(errors)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred",
                    Status = (int)HttpStatusCode.BadRequest,
                };
            }
            // Custom ValidationException
            else if (exception is ValidationException ve)
            {
                problemDetails = new ValidationProblemDetails(ve.Errors)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred",
                    Status = (int)HttpStatusCode.BadRequest,
                };
            }
            // Custom NotFoundException
            else if (exception is NotFoundException nfe)
            {
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Resource not found",
                    Status = (int)HttpStatusCode.NotFound,
                };
            }
            // Custom ConflictException
            else if (exception is ConflictException ce)
            {
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    Title = "The resource is in a conflict state",
                    Status = (int)HttpStatusCode.Conflict,
                };
            }
            // Custom UnauthorizedException
            else if (exception is UnauthorizedException ue)
            {
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                    Title = "Unauthorized",
                    Status = (int)HttpStatusCode.Unauthorized,
                };
            }
            // Catch all exception handler
            else
            {
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "An unexpected error occurred",
                    Status = (int)HttpStatusCode.InternalServerError,
                };

                string bodyText;
                using (var bodyReader = new StreamReader(context.Request.Body))
                {
                    bodyText = await bodyReader.ReadToEndAsync();
                }
                logger.LogError(exception, "The following request failed, Method: {method}, URL: {path} request body: {body}",
                    context.Request.Method, context.Request.Path, bodyText);

            }

            problemDetails.Detail = GetMessage(exception);
            problemDetails.Instance = context.Request.Path;

            var result = JsonConvert.SerializeObject(problemDetails,
                                                     Formatting.None,
                                                     new JsonSerializerSettings
                                                     {
                                                         NullValueHandling = NullValueHandling.Ignore,
                                                         ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                     });

            context.Response.StatusCode = problemDetails.Status!.Value;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsync(result);
        }

        private static string GetMessage(Exception exception)
        {
            var sb = new StringBuilder();
            var message = exception.Message;
            sb.Append(message);
            var ex = exception.InnerException;
            while (ex != null)
            {
                if (ex.Message != message)
                {
                    message = exception.Message;
                    sb.Append($" -*- {message}");
                }
                ex = ex.InnerException;
            }
            return sb.ToString();
        }
    }

}
