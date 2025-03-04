namespace Events_WEB_APP.Infrastructure.PasswordHashers
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        public bool Verify(string password, string hashedPassword);
    }
}