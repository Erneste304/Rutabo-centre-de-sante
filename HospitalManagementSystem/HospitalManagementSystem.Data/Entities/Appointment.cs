using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string AppointmentNumber { get; set; } = string.Empty;
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        public int? DepartmentId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        [Required]
        public TimeSpan AppointmentTime { get; set; }
        
        [Required]
        [StringLength(50)]
        public string AppointmentType { get; set; } = "Consultation"; // Consultation, Follow-up, Emergency, Check-up, Surgery
        
        [StringLength(20)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Confirmed, Completed, Cancelled, No-Show, Rescheduled
        
        [StringLength(500)]
        public string? Reason { get; set; }
        
        [StringLength(500)]
        public string? Symptoms { get; set; }
        
        [StringLength(500)]
        public string? Diagnosis { get; set; }
        
        [Column(TypeName = "TEXT")]
        public string? Prescription { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public int DurationMinutes { get; set; } = 30;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int? CreatedBy { get; set; }
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
       
        [ForeignKey("PatientId")]
        public virtual HospitalManagementSystem.Data.Entities.Patient Patient { get; set; } = null!;
        
        [ForeignKey("DoctorId")]
        public virtual HospitalManagementSystem.Data.Entities.Doctor Doctor { get; set; } = null!;
        
        [ForeignKey("DepartmentId")]
        public virtual HospitalManagementSystem.Data.Entities.Department? Department { get; set; }
        
        [ForeignKey("CreatedBy")]
        public virtual HospitalManagementSystem.Data.Entities.User? CreatedByUser { get; set; }
        
        public virtual HospitalManagementSystem.Data.Entities.Billing? Billing { get; set; }
        public virtual ICollection<HospitalManagementSystem.Data.Entities.MedicalRecord> MedicalRecords { get; set; } = new List<HospitalManagementSystem.Data.Entities.MedicalRecord>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Prescription> Prescriptions { get; set; } = new List<HospitalManagementSystem.Data.Entities.Prescription>();
    }
}