namespace EduPlay.WebAPI.Auth
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
    }
}
