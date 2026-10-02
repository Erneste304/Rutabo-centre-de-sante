using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Repositories;
using HospitalManagementSystem.Core.Services;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystem.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Register_ValidUser_ReturnsUser()
        {
            var repository = new InMemoryUserRepository();
            var service = new UserService(repository);
            var user = new User
            {
                Username = "testuser",
                Email = "test@email.com",
                UserType = UserType.Patient
            };

            var registeredUser = await service.RegisterAsync(user, "correct-password");

            Assert.Same(user, registeredUser);
            Assert.True(registeredUser.IsActive);
            Assert.NotEqual("correct-password", registeredUser.PasswordHash);
            Assert.StartsWith("pbkdf2-sha256$", registeredUser.PasswordHash);
            Assert.Equal(user, await repository.GetByUsernameAsync("testuser"));
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsUser()
        {
            var repository = new InMemoryUserRepository();
            var service = new UserService(repository);
            var user = new User { Username = "testuser", UserType = UserType.Patient };
            await service.RegisterAsync(user, "correct-password");

            var authenticatedUser = await service.AuthenticateAsync("testuser", "correct-password");

            Assert.Same(user, authenticatedUser);
        }

        [Fact]
        public async Task Authenticate_InvalidPassword_ReturnsNull()
        {
            var repository = new InMemoryUserRepository();
            var service = new UserService(repository);
            var user = new User { Username = "testuser", UserType = UserType.Patient };
            await service.RegisterAsync(user, "correct-password");

            var authenticatedUser = await service.AuthenticateAsync("testuser", "wrong-password");

            Assert.Null(authenticatedUser);
        }

        [Fact]
        public async Task Register_NonPatientUser_RequiresActivation()
        {
            var repository = new InMemoryUserRepository();
            var service = new UserService(repository);
            var user = new User { Username = "new-doctor", UserType = UserType.Doctor };

            await service.RegisterAsync(user, "correct-password");

            Assert.False(user.IsActive);
            Assert.Null(await service.AuthenticateAsync("new-doctor", "correct-password"));
        }

        [Fact]
        public async Task Authenticate_LegacyPasswordHash_RehashesAfterSuccessfulLogin()
        {
            var repository = new InMemoryUserRepository();
            var service = new UserService(repository);
            var password = "correct-password";
            var legacyHash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(password)));
            var user = new User
            {
                Username = "legacy-user",
                PasswordHash = legacyHash,
                UserType = UserType.Patient
            };
            await repository.AddAsync(user);

            var authenticatedUser = await service.AuthenticateAsync(user.Username, password);

            Assert.Same(user, authenticatedUser);
            Assert.StartsWith("pbkdf2-sha256$", user.PasswordHash);
            Assert.True(PasswordHasher.VerifyPassword(password, user.PasswordHash));
        }

        [Fact]
        public void PasswordHasher_UsesUniqueSaltsAndRejectsWrongPassword()
        {
            var firstHash = PasswordHasher.HashPassword("correct-password");
            var secondHash = PasswordHasher.HashPassword("correct-password");

            Assert.NotEqual(firstHash, secondHash);
            Assert.True(PasswordHasher.VerifyPassword("correct-password", firstHash));
            Assert.False(PasswordHasher.VerifyPassword("wrong-password", firstHash));
        }

        private sealed class InMemoryUserRepository : IUserRepository
        {
            private readonly Dictionary<string, User> _users = new();
            private int _nextId = 1;

            public Task<User?> GetByUsernameAsync(string username) =>
                Task.FromResult(_users.GetValueOrDefault(username));

            public Task AddAsync(User user)
            {
                user.UserId = _nextId++;
                _users.Add(user.Username, user);
                return Task.CompletedTask;
            }

            public Task<User?> GetByIdAsync(int id) =>
                Task.FromResult(_users.Values.FirstOrDefault(user => user.UserId == id));

            public Task UpdateAsync(User user)
            {
                _users[user.Username] = user;
                return Task.CompletedTask;
            }

            public Task<bool> ExistsByUsernameOrEmailAsync(string username, string email) =>
                Task.FromResult(_users.Values.Any(user =>
                    user.Username == username || user.Email == email));
        }
    }
}