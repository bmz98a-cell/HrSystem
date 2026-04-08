namespace HRSystem.API.Services
{
    public class PasswordService
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string input, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(input, hash);
        }
    }
}
