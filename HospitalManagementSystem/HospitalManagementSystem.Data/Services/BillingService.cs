using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public class BillingService : IBillingService
    {
        private readonly ApplicationDbContext _context;

        public BillingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Billing>> GetAllBillingsAsync()
        {
            return await _context.Billings
                .Include(b => b.Patient)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync();
        }

        public async Task<Billing?> GetBillingByIdAsync(int id)
        {
            return await _context.Billings
                .Include(b => b.Patient)
                .Include(b => b.BillItems)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.BillId == id);
        }

        public async Task<Billing> CreateBillingAsync(Billing billing)
        {
            billing.CreatedAt = DateTime.UtcNow;
            billing.BillNumber = "INV-" + DateTime.UtcNow.Ticks.ToString().Substring(10);
            _context.Billings.Add(billing);
            await _context.SaveChangesAsync();
            return billing;
        }

        public async Task<Payment> AddPaymentAsync(int billId, Payment payment)
        {
            var bill = await _context.Billings.FindAsync(billId);
            if (bill == null) throw new Exception("Bill not found");

            payment.BillId = billId;
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentNumber = "PAY-" + DateTime.UtcNow.Ticks.ToString().Substring(10);
            
            _context.Payments.Add(payment);
            
            // Update bill status
            bill.PaidAmount += payment.Amount;
            bill.BalanceDue = bill.TotalAmount - bill.PaidAmount;
            
            if (bill.BalanceDue <= 0)
                bill.PaymentStatus = "Paid";
            else if (bill.PaidAmount > 0)
                bill.PaymentStatus = "Partially Paid";

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Billing>> GetBillingsByPatientIdAsync(int patientId)
        {
            return await _context.Billings
                .Where(b => b.PatientId == patientId)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync();
        }
    }
}
