using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicalRecord>> GetPatientHistoryAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(r => r.Doctor)
                .ThenInclude(d => d.User)
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToListAsync();
        }

        public async Task<MedicalRecord?> GetRecordByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .FirstOrDefaultAsync(r => r.RecordId == id);
        }

        public async Task<MedicalRecord> AddRecordAsync(MedicalRecord record)
        {
            record.CreatedAt = DateTime.UtcNow;
            if (record.VisitDate == default) record.VisitDate = DateTime.UtcNow;
            
            _context.MedicalRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<IEnumerable<MedicalRecord>> GetCriticalRecordsAsync()
        {
            return await _context.MedicalRecords
                .Include(r => r.Patient)
                .Where(r => r.IsCritical)
                .OrderByDescending(r => r.VisitDate)
                .ToListAsync();
        }
    }

    public class LabTestService : ILabTestService
    {
        private readonly ApplicationDbContext _context;

        public LabTestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LabTest>> GetPendingTestsAsync()
        {
            return await _context.LabTests
                .Include(t => t.Patient)
                .Include(t => t.Doctor)
                .Where(t => t.Status == "Pending")
                .OrderBy(t => t.TestDate)
                .ToListAsync();
        }

        public async Task<LabTest?> GetTestByIdAsync(int id)
        {
            return await _context.LabTests
                .Include(t => t.Patient)
                .Include(t => t.Doctor)
                .FirstOrDefaultAsync(t => t.LabTestId == id);
        }

        public async Task<LabTest> OrderTestAsync(LabTest test)
        {
            test.TestDate = DateTime.UtcNow;
            test.Status = "Pending";
            _context.LabTests.Add(test);
            await _context.SaveChangesAsync();
            return test;
        }

        public async Task<bool> SubmitResultAsync(int testId, string result, string performedBy)
        {
            var test = await _context.LabTests.FindAsync(testId);
            if (test == null) return false;

            test.Result = result;
            test.Status = "Completed";
            test.PerformedBy = performedBy;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<LabTest>> GetPatientTestsAsync(int patientId)
        {
            return await _context.LabTests
                .Where(t => t.PatientId == patientId)
                .OrderByDescending(t => t.TestDate)
                .ToListAsync();
        }
    }
}
