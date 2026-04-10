using HospitalManagementSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface IBillingService
    {
        Task<IEnumerable<Billing>> GetAllBillingsAsync();
        Task<Billing?> GetBillingByIdAsync(int id);
        Task<Billing> CreateBillingAsync(Billing billing);
        Task<Payment> AddPaymentAsync(int billId, Payment payment);
        Task<IEnumerable<Billing>> GetBillingsByPatientIdAsync(int patientId);
    }
}
