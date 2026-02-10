using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Schedules")]
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        [Range(1, 7)]
        public int DayOfWeek { get; set; } 
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        public int SlotDuration { get; set; } = 30; 
        
        public int MaxAppointments { get; set; } = 10;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        
        [ForeignKey("DoctorId")]
        public virtual HospitalManagementSystem.Data.Entities.Doctor Doctor { get; set; } = null!;
    }
}