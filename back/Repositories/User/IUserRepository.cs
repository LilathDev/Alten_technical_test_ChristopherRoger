using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Models.Entities;

namespace back.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string username, string email);
    }
}