using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("MedicalRecords")]
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }

        public int? AppointmentId { get; set; }
        
        public DateTime VisitDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(50)]
        public string RecordType { get; set; } = "Consultation"; // Consultation, Lab Test, X-Ray, Surgery, Emergency, Follow-up
        
        [StringLength(1000)]
        public string? Symptoms { get; set; }
        
        [StringLength(1000)]
        public string? Diagnosis { get; set; }
        
        [StringLength(1000)]
        public string? Treatment { get; set; }
        
        [StringLength(1000)]
        public string? Medications { get; set; }
        
        [StringLength(500)]
        public string? Dosage { get; set; }
        
        [StringLength(100)]
        public string? Duration { get; set; }
        
        [Column(TypeName = "TEXT")]
        public string? LabResults { get; set; }
        
        [StringLength(500)]
        public string? VitalSigns { get; set; }
        
        [Column(TypeName = "TEXT")]
        public string? Notes { get; set; }
        
        public DateTime? FollowUpDate { get; set; }
        
        public bool IsCritical { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int? CreatedBy { get; set; }
        
        
        [ForeignKey("PatientId")]
        public virtual HospitalManagementSystem.Data.Entities.Patient Patient { get; set; } = null!;
        
        [ForeignKey("DoctorId")]
        public virtual HospitalManagementSystem.Data.Entities.Doctor Doctor { get; set; } = null!;
        
        [ForeignKey("AppointmentId")]
        public virtual HospitalManagementSystem.Data.Entities.Appointment? Appointment { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual HospitalManagementSystem.Data.Entities.User? CreatedByUser { get; set; }

        public virtual ICollection<HospitalManagementSystem.Data.Entities.MedicalRecordMedicine> MedicalRecordMedicines { get; set; } = new List<HospitalManagementSystem.Data.Entities.MedicalRecordMedicine>();
    }
}