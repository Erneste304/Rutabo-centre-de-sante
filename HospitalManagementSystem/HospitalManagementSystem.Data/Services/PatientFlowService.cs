using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public class PatientFlowService : IPatientFlowService
    {
        private readonly ApplicationDbContext _context;

        public PatientFlowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PatientVisit> StartVisitAsync(int patientId, string visitType, string initialComplaint)
        {
            var visitNumber = $"VIS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
            
            var visit = new PatientVisit
            {
                PatientId = patientId,
                VisitNumber = visitNumber,
                VisitType = visitType,
                InitialComplaint = initialComplaint,
                Status = "Registered",
                VisitDate = DateTime.UtcNow,
                Priority = visitType == "Emergency" ? "High" : "Normal"
            };

            _context.PatientVisits.Add(visit);
            await _context.SaveChangesAsync();
            return visit;
        }

        public async Task<IEnumerable<PatientVisit>> GetActiveVisitsAsync()
        {
            return await _context.PatientVisits
                .Include(v => v.Patient)
                .Include(v => v.TriageRecord)
                .Where(v => v.Status != "Completed")
                .OrderByDescending(v => v.Priority == "High")
                .ThenBy(v => v.VisitDate)
                .ToListAsync();
        }

        public async Task<PatientVisit?> GetVisitByIdAsync(int visitId)
        {
            return await _context.PatientVisits
                .Include(v => v.Patient)
                .Include(v => v.TriageRecord)
                .FirstOrDefaultAsync(v => v.VisitId == visitId);
        }

        public async Task<TriageRecord> RecordTriageAsync(TriageRecord record)
        {
            _context.TriageRecords.Add(record);
            
            // Auto-update visit status
            var visit = await _context.PatientVisits.FindAsync(record.VisitId);
            if (visit != null)
            {
                visit.Status = "WaitingForDoctor";
                // Simple auto-priority logic
                if (record.Temperature > 39 || record.BloodPressureSystolic > 160)
                {
                    visit.Priority = "High";
                }
            }

            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<IEnumerable<InsuranceType>> GetInsuranceTypesAsync()
        {
            return await _context.InsuranceTypes.Where(i => i.IsActive).ToListAsync();
        }

        public async Task<bool> UpdateVisitStatusAsync(int visitId, string status, string? priority = null)
        {
            var visit = await _context.PatientVisits.FindAsync(visitId);
            if (visit == null) return false;

            visit.Status = status;
            if (priority != null) visit.Priority = priority;
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
