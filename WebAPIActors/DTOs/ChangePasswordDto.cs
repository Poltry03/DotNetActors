namespace WebAPIActors.DTOs
{
    public class ChangePasswordDto
    {
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}
