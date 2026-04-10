using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _context.Inventory.OrderBy(i => i.ItemName).ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync()
        {
            return await _context.Inventory
                .Where(i => i.CurrentStock <= i.MinimumStock && i.Status == "Active")
                .ToListAsync();
        }

        public async Task<Inventory?> GetItemByIdAsync(int id)
        {
            return await _context.Inventory.FindAsync(id);
        }

        public async Task<Inventory> AddItemAsync(Inventory item)
        {
            item.LastRestocked = DateTime.UtcNow;
            _context.Inventory.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateStockAsync(int id, int quantity)
        {
            var item = await _context.Inventory.FindAsync(id);
            if (item == null) return false;

            item.CurrentStock += quantity;
            item.LastRestocked = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Medicine>> GetAllMedicinesAsync()
        {
            return await _context.Medicines.OrderBy(m => m.Name).ToListAsync();
        }
    }

    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prescription>> GetActivePrescriptionsAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .ThenInclude(d => d.User)
                .Include(p => p.PrescriptionItems)
                .Where(p => p.Status == "Active")
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();
        }

        public async Task<Prescription?> GetPrescriptionByIdAsync(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .ThenInclude(d => d.User)
                .Include(p => p.PrescriptionItems)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);
        }

        public async Task<Prescription> CreatePrescriptionAsync(Prescription prescription)
        {
            prescription.CreatedAt = DateTime.UtcNow;
            prescription.PrescriptionDate = DateTime.UtcNow;
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task<bool> DispensePrescriptionAsync(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptionItems)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescription == null || prescription.Status != "Active") return false;

            // Logic to deduct from medicine stock
            foreach (var item in prescription.PrescriptionItems)
            {
                var medicine = await _context.Medicines
                    .FirstOrDefaultAsync(m => m.Name == item.MedicationName);
                
                if (medicine != null && item.Quantity.HasValue)
                {
                    medicine.StockQuantity -= item.Quantity.Value;
                }
            }

            prescription.Status = "Dispensed";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Prescription>> GetPatientPrescriptionsAsync(int patientId)
        {
            return await _context.Prescriptions
                .Where(p => p.PatientId == patientId)
                .Include(p => p.PrescriptionItems)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();
        }
    }
}
