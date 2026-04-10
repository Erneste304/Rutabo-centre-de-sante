using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Repositories;
using HospitalManagementSystem.Data;

namespace HospitalManagementSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HospitalManagementSystem.Core.Models.User?> GetByUsernameAsync(string username)
        {
            var entity = await _context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(u => u.Username == username);
            
            if (entity == null) return null;
            
            return MapToModel(entity);
        }

        public async Task AddAsync(HospitalManagementSystem.Core.Models.User user)
        {
            var entity = new HospitalManagementSystem.Data.Entities.User
            {
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                UserType = user.UserType.ToString(),
                FullName = user.FullName,
                Status = user.IsActive ? "Active" : "Inactive",

                ResetRequested = user.ResetRequested,
                CreatedAt = user.CreatedAt
            };
            
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            user.UserId = entity.UserId;
        }

        public async Task<HospitalManagementSystem.Core.Models.User?> GetByIdAsync(int id)
        {
            var entity = await _context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(u => u.UserId == id);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task UpdateAsync(HospitalManagementSystem.Core.Models.User user)
        {
            var entity = await _context.Users.FindAsync(user.UserId);
            if (entity != null)
            {
                entity.Username = user.Username;
                entity.Email = user.Email;
                entity.PasswordHash = user.PasswordHash;
                entity.UserType = user.UserType.ToString();
                entity.Status = user.IsActive ? "Active" : "Inactive";
                entity.ResetRequested = user.ResetRequested;
                
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByUsernameOrEmailAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
        }
        
        private HospitalManagementSystem.Core.Models.User MapToModel(HospitalManagementSystem.Data.Entities.User entity)
        {
            Enum.TryParse<HospitalManagementSystem.Core.Models.UserType>(entity.UserType, out var userType);
            
            return new HospitalManagementSystem.Core.Models.User
            {
                UserId = entity.UserId,
                Username = entity.Username,
                FullName = entity.FullName,
                Email = entity.Email,

                PasswordHash = entity.PasswordHash,
                UserType = userType,
                IsActive = entity.Status == "Active",
                DoctorId = entity.Doctor?.DoctorId,
                PatientId = entity.Patient?.PatientId,
                ResetRequested = entity.ResetRequested,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
