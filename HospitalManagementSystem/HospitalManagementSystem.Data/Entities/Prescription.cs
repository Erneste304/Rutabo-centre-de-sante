using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Prescriptions")]
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        public int? AppointmentId { get; set; }
        
        public DateTime PrescriptionDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(500)]
        public string? Diagnosis { get; set; }
        
        [Column(TypeName = "TEXT")]
        public string? Instructions { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Active"; 
        
        public DateTime? ValidUntil { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        
        [ForeignKey("PatientId")]
        public virtual HospitalManagementSystem.Data.Entities.Patient Patient { get; set; } = null!;
        
        [ForeignKey("DoctorId")]
        public virtual HospitalManagementSystem.Data.Entities.Doctor Doctor { get; set; } = null!;
        
        [ForeignKey("AppointmentId")]
        public virtual HospitalManagementSystem.Data.Entities.Appointment? Appointment { get; set; }
        
        public virtual ICollection<HospitalManagementSystem.Data.Entities.PrescriptionItem> PrescriptionItems { get; set; } = new List<HospitalManagementSystem.Data.Entities.PrescriptionItem>();
    }
}