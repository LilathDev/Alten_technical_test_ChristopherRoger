using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back.Services
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}