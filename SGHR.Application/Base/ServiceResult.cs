namespace SGHR.Application.Base
{
    public class ServiceResult
    {
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
        public dynamic? Data { get; set; }
    }

}
