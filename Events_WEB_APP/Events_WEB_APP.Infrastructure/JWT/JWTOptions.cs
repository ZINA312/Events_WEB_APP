namespace Events_WEB_APP.Infrastructure.JWT
{
    public class JWTOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
    }
}
