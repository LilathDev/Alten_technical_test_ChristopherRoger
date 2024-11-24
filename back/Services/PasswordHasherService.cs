using back.Services;
using Microsoft.AspNetCore.Identity;

namespace back.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<string> _passwordHasher;

        public PasswordHasherService()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}