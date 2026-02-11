using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    public class EmployeeShift
    {
        [Key]
        public int ShiftId { get; set; }

        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        public DateTime ShiftDate { get; set; }

        [Required]
        [StringLength(50)]
        public string? ShiftType { get; set; } // Morning, Evening, Night, General

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public int? DepartmentId { get; set; }
        
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Working, Completed, Cancelled

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
