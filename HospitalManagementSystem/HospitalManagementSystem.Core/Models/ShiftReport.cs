using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Core.Models
{
    [Table("ShiftReports")]
    public class ShiftReport
    {
        [Key]
        public int ReportId { get; set; }
        
        [Required]
        public int NurseId { get; set; } // UserId of the nurse
        
        [Required]
        public int ShiftId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string ShiftType { get; set; } = "Morning"; // Morning, Evening, Night
        
        [Required]
        public DateTime ShiftDate { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        [StringLength(500)]
        public string? PatientsHandled { get; set; } // Could be JSON or comma-separated
        
        [StringLength(1000)]
        public string? TasksCompleted { get; set; }
        
        [StringLength(1000)]
        public string? MedicationsAdministered { get; set; }
        
        [StringLength(1000)]
        public string? CriticalIncidents { get; set; }
        
        [StringLength(1000)]
        public string? IssuesConcerns { get; set; }
        
        [StringLength(1000)]
        public string? HandoverNotes { get; set; }
        
        [StringLength(1000)]
        public string? NextShiftNotes { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? SubmittedAt { get; set; }
        
        
        [ForeignKey("NurseId")]
        public virtual User Nurse { get; set; } = null!;
        
        [ForeignKey("ShiftId")]
        public virtual EmployeeShift? Shift { get; set; }
    }
}