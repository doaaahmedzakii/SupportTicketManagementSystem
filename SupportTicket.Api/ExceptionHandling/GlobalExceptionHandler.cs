using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SupportTicket.Application.Exceptions;

namespace SupportTicket.Api.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An Unexpected error , trace id : {TraceId}", httpContext.TraceIdentifier);
            var statusCode = exception switch
            {
                TicketNotFoundException => StatusCodes.Status404NotFound,
                InvalidStatusTransitionException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
            var title = exception switch
            {
                TicketNotFoundException => "ticket not found",
                InvalidStatusTransitionException => "Invalid Status transition",
                _ => "Internal server Error"
            };
            var detail = exception switch
            {
                TicketNotFoundException => exception.Message,
                InvalidStatusTransitionException => exception.Message,
                _ => "An Unexpected error"
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
