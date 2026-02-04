using Xunit;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Services;

namespace HospitalManagementSystem.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Register_ValidUser_ReturnsUser()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@email.com",
                UserType = UserType.Patient
            };
            
           
        }
        
        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsUser()
        {
            // Test authentication logic
        }
    }
}