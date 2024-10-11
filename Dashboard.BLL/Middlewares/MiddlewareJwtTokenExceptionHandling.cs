using Dashboard.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Dashboard.BLL.Middlewares
{
    public class MiddlewareJwtTokenExceptionHandling
    {
        private readonly RequestDelegate _next;

        public MiddlewareJwtTokenExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SecurityTokenException ex)
            {
                string error = ex.Message;
                var response = ServiceResponse.GetInternalServerErrorResponse(message: ex.Message, errors: error);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
