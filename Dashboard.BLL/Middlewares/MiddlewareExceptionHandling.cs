using Dashboard.BLL.Services;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Dashboard.BLL.Middlewares
{
    public class MiddlewareExceptionHandling
    {
        private readonly RequestDelegate _next;

        public MiddlewareExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = ServiceResponse.GetInternalServerErrorResponse(message: ex.Message, errors: error);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
