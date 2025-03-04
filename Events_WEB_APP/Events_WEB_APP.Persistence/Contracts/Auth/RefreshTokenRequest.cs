namespace Events_WEB_APP.Persistence.Contracts.Auth
{
    public record RefreshTokenRequest(string AccessToken, string RefreshToken);
}
