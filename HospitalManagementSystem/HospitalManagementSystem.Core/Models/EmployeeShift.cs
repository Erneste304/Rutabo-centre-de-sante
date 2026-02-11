using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Core.Models
{
    [Table("EmployeeShifts")]
    public class EmployeeShift
    {
        [Key]
        public int ShiftId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(50)]
        public string ShiftType { get; set; } = string.Empty; // Morning, Evening, Night

        public DateTime ShiftDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
