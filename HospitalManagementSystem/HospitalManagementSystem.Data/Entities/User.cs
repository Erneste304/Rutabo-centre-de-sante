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
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLogin { get; set; }
        
        [StringLength(500)]
        public string? ProfileImage { get; set; }
        
        public bool IsEmailVerified { get; set; } = false;
        
        public bool TwoFactorEnabled { get; set; } = false;
        
        // Navigation properties
        public virtual Patient? Patient { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public virtual ICollection<Appointment> AppointmentsCreated { get; set; } = new List<Appointment>();
        public virtual ICollection<MedicalRecord> MedicalRecordsCreated { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Billing> BillsCreated { get; set; } = new List<Billing>();
        public virtual ICollection<Payment> PaymentsReceived { get; set; } = new List<Payment>();
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}