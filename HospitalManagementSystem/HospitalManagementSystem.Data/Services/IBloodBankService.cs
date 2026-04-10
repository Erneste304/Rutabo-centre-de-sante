using HospitalManagementSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface IBloodBankService
    {
        Task<IEnumerable<BloodBank>> GetBloodStockAsync();
        Task<bool> UpdateStockAsync(string bloodType, int delta);
        Task<BloodBank?> GetByBloodTypeAsync(string bloodType);
    }
}
