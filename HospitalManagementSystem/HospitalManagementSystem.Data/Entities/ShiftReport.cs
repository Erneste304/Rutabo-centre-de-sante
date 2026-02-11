using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("ShiftReports")]
    public class ShiftReport
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public int NurseId { get; set; }
        
        [ForeignKey("NurseId")]
        public virtual User? Nurse { get; set; }

        [Required]
        public DateTime ShiftDate { get; set; }

        [StringLength(50)]
        public string? ShiftType { get; set; } // Morning, Evening, Night, General
        
        public TimeSpan? StartTime { get; set; }
        
        public TimeSpan? EndTime { get; set; }

        public string? PatientsHandled { get; set; } // Can be a comma-separated list or JSON
        
        public int TasksCompleted { get; set; }
        
        public int MedicationsAdministered { get; set; }
        
        public string? CriticalIncidents { get; set; }
        
        public string? IssuesConcerns { get; set; }
        
        public string? HandoverNotes { get; set; }
        
        public string? NextShiftNotes { get; set; }
        
        public bool IsCompleted { get; set; }
        
        public DateTime? SubmittedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
