namespace Dashboard.BLL.Services
{
    public class ServiceResponse<T>
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Payload { get; set; }

        public static ServiceResponse<T> GetServiceResponse(string? message, bool success, T? payload, params string[] errors)
        {
            return new ServiceResponse<T>
            {
                Success = success,
                Message = message,
                Payload = payload,
                Errors = new List<string>(errors)
            };
        }
    }
}
