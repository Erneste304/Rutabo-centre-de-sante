using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string UserType { get; set; } = string.Empty; // Admin, Doctor, Nurse, Patient, Receptionist, Accountant
        
        [StringLength(100)]
        public string? Specialization { get; set; }
        
        [StringLength(100)]
        public string? Department { get; set; }
        
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(255)]
        public string? Address { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; } // Male, Female, Other
        
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive, Pending, Suspended

        [NotMapped]
        public bool IsActive 
        { 
            get => Status == "Active";
            set => Status = value ? "Active" : "Inactive";
        }
        
        public bool ResetRequested { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public DateTime? LastLogin { get; set; }
        
        [StringLength(500)]
        public string? ProfileImage { get; set; }
        
        public bool IsEmailVerified { get; set; } = false;
        
        public bool TwoFactorEnabled { get; set; } = false;
        
        // Navigation properties
        public virtual HospitalManagementSystem.Data.Entities.Patient? Patient { get; set; }
        public virtual HospitalManagementSystem.Data.Entities.Doctor? Doctor { get; set; }
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Appointment> AppointmentsCreated { get; set; } = new List<HospitalManagementSystem.Data.Entities.Appointment>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.MedicalRecord> MedicalRecordsCreated { get; set; } = new List<HospitalManagementSystem.Data.Entities.MedicalRecord>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Billing> BillsCreated { get; set; } = new List<HospitalManagementSystem.Data.Entities.Billing>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Payment> PaymentsReceived { get; set; } = new List<HospitalManagementSystem.Data.Entities.Payment>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.AuditLog> AuditLogs { get; set; } = new List<HospitalManagementSystem.Data.Entities.AuditLog>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Notification> Notifications { get; set; } = new List<HospitalManagementSystem.Data.Entities.Notification>();
    }
}