using System.Net;

namespace Dashboard.BLL.Services
{
    public class ServiceResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new();
        public object? Payload { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ServiceResponse GetServiceResponse(string? message, bool success, object? payload, HttpStatusCode statusCode, params string[] errors)
        {
            return new ServiceResponse
            {
                Success = success,
                Message = message,
                Payload = payload,
                StatusCode = statusCode,
                Errors = new List<string>(errors)
            };
        }

        public static ServiceResponse GetOkResponse(string? message, object? payload = null)
        {
            return GetServiceResponse(message, true, payload, HttpStatusCode.OK);
        }

        public static ServiceResponse GetBadRequestResponse(string? message, object? payload = null, params string[] errors)
        {
            return GetServiceResponse(message, false, payload, HttpStatusCode.BadRequest, errors);
        }

        public static ServiceResponse GetInternalServerErrorResponse(string? message, object? payload = null, params string[] errors)
        {
            return GetServiceResponse(message, false, payload, HttpStatusCode.InternalServerError, errors);
        }
    }
}
