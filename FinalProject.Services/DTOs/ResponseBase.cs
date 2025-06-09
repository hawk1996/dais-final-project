namespace FinalProject.Services.DTOs
{
    public class ResponseBase
    {
        public bool Success { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }
}
