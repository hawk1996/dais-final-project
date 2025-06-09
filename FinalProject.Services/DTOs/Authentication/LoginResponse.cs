namespace FinalProject.Services.DTOs.Authentication
{
    public class LoginResponse : ResponseBase
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
    }
}
