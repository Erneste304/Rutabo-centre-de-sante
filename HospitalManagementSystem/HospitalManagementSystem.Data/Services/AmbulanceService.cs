using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public class AmbulanceService : IAmbulanceService
    {
        private readonly ApplicationDbContext _context;

        public AmbulanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ambulance>> GetFleetAsync()
        {
            return await _context.Ambulances
                .Include(a => a.Driver)
                .ToListAsync();
        }

        public async Task<Ambulance?> GetAmbulanceByIdAsync(int id)
        {
            return await _context.Ambulances
                .Include(a => a.Driver)
                .FirstOrDefaultAsync(a => a.AmbulanceId == id);
        }

        public async Task<IEnumerable<AmbulanceLog>> GetActiveLogsAsync()
        {
            return await _context.AmbulanceLogs
                .Include(l => l.Ambulance)
                .Include(l => l.Patient)
                .Where(l => l.Status == "Pending" || l.Status == "InProgress")
                .OrderByDescending(l => l.PickupTime)
                .ToListAsync();
        }

        public async Task<AmbulanceLog> DispatchAmbulanceAsync(AmbulanceLog log)
        {
            log.Status = "InProgress";
            log.PickupTime = DateTime.UtcNow;
            
            _context.AmbulanceLogs.Add(log);
            
            // Update ambulance availability
            var ambulance = await _context.Ambulances.FindAsync(log.AmbulanceId);
            if (ambulance != null)
            {
                ambulance.Availability = "Dispatched";
            }
            
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<bool> UpdateLogStatusAsync(int logId, string status, DateTime? timestamp = null)
        {
            var log = await _context.AmbulanceLogs.FindAsync(logId);
            if (log == null) return false;

            log.Status = status;
            if (status == "Completed")
            {
                log.DropoffTime = timestamp ?? DateTime.UtcNow;
                
                // Set ambulance back to available
                var ambulance = await _context.Ambulances.FindAsync(log.AmbulanceId);
                if (ambulance != null)
                {
                    ambulance.Availability = "Available";
                }
            }
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAmbulanceStatusAsync(int ambulanceId, string status)
        {
            var ambulance = await _context.Ambulances.FindAsync(ambulanceId);
            if (ambulance == null) return false;

            ambulance.Availability = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
