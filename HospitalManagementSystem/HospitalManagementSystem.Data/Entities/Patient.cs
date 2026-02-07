using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Patients")]
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string MedicalRecordNumber { get; set; } = string.Empty;
        
        [StringLength(5)]
        public string? BloodType { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; } // in cm
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; } // in kg
        
        [StringLength(100)]
        public string? EmergencyContactName { get; set; }
        
        [StringLength(15)]
        public string? EmergencyContactPhone { get; set; }
        
        [StringLength(50)]
        public string? EmergencyContactRelation { get; set; }
        
        [StringLength(100)]
        public string? PrimaryCarePhysician { get; set; }
        
        [StringLength(100)]
        public string? InsuranceProvider { get; set; }
        
        [StringLength(50)]
        public string? InsurancePolicyNumber { get; set; }
        
        [StringLength(500)]
        public string? Allergies { get; set; }
        
        [StringLength(500)]
        public string? ChronicConditions { get; set; }
        
        [StringLength(500)]
        public string? CurrentMedications { get; set; }
        
        public DateTime AdmissionDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? DischargeDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Billing> Bills { get; set; } = new List<Billing>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<LabTest> LabTests { get; set; } = new List<LabTest>();
    }
}