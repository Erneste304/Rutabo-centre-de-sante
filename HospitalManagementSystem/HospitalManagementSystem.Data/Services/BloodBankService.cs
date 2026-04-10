using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public class BloodBankService : IBloodBankService
    {
        private readonly ApplicationDbContext _context;

        public BloodBankService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BloodBank>> GetBloodStockAsync()
        {
            var stock = await _context.BloodBanks.ToListAsync();
            
            // If empty, seed initial blood types
            if (!stock.Any())
            {
                var types = new[] { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };
                foreach (var t in types)
                {
                    _context.BloodBanks.Add(new BloodBank { BloodType = t, StockQuantity = 0, LastUpdated = DateTime.UtcNow });
                }
                await _context.SaveChangesAsync();
                stock = await _context.BloodBanks.ToListAsync();
            }
            
            return stock.OrderBy(s => s.BloodType);
        }

        public async Task<bool> UpdateStockAsync(string bloodType, int delta)
        {
            var entry = await _context.BloodBanks.FirstOrDefaultAsync(b => b.BloodType == bloodType);
            if (entry == null) return false;

            entry.StockQuantity += delta;
            if (entry.StockQuantity < 0) entry.StockQuantity = 0;
            entry.LastUpdated = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BloodBank?> GetByBloodTypeAsync(string bloodType)
        {
            return await _context.BloodBanks.FirstOrDefaultAsync(b => b.BloodType == bloodType);
        }
    }
}
