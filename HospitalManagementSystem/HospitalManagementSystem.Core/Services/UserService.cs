using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Repositories;

namespace HospitalManagementSystem.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            
            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash) || !user.IsActive)
                return null;

            if (PasswordHasher.NeedsRehash(user.PasswordHash))
            {
                user.PasswordHash = PasswordHasher.HashPassword(password);
                await _userRepository.UpdateAsync(user);
            }

            return user;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            user.PasswordHash = PasswordHasher.HashPassword(password);
            // Non-patient users require admin approval
            user.IsActive = user.UserType == UserType.Patient;
            await _userRepository.AddAsync(user);
            return user;
        }



        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<bool> UserExistsAsync(string username, string email)

        {
            return await _userRepository.ExistsByUsernameOrEmailAsync(username, email);
        }
    }
}