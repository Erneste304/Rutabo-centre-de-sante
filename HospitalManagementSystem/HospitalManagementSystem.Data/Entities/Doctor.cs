using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Doctors")]
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Qualifications { get; set; }
        
        public int YearsOfExperience { get; set; } = 0;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal ConsultationFee { get; set; } = 0;
        
        [StringLength(100)]
        public string? AvailableDays { get; set; } // e.g., "Mon,Wed,Fri"
        
        [StringLength(50)]
        public string? WorkingHours { get; set; } // e.g., "9:00-17:00"
        
        public int MaxPatientsPerDay { get; set; } = 20;
        
        public bool IsAvailable { get; set; } = true;
        
        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; } = 0;
        
        public int TotalRatings { get; set; } = 0;
        
        [Column(TypeName = "TEXT")]
        public string? Biography { get; set; }
        
        
        [ForeignKey("UserId")]
        public virtual HospitalManagementSystem.Data.Entities.User User { get; set; } = null!;
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Appointment> Appointments { get; set; } = new List<HospitalManagementSystem.Data.Entities.Appointment>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.MedicalRecord> MedicalRecords { get; set; } = new List<HospitalManagementSystem.Data.Entities.MedicalRecord>();
        public virtual HospitalManagementSystem.Data.Entities.Department? Department { get; set; }
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Schedule> Schedules { get; set; } = new List<HospitalManagementSystem.Data.Entities.Schedule>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Prescription> Prescriptions { get; set; } = new List<HospitalManagementSystem.Data.Entities.Prescription>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.DoctorDepartment> DoctorDepartments { get; set; } = new List<HospitalManagementSystem.Data.Entities.DoctorDepartment>();
    }
}