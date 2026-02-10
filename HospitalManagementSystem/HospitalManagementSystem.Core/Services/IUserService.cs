using HospitalManagementSystem.Core.Models;

namespace HospitalManagementSystem.Core.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(User user, string password);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task UpdateUserAsync(User user);

        Task<bool> UserExistsAsync(string username, string email);
    }
}