using HospitalManagementSystem.Core.Models;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task<bool> ExistsByUsernameOrEmailAsync(string username, string email);
    }
}
