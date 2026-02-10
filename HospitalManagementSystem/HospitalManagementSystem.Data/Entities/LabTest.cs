using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("LabTests")]
    public class LabTest
    {
        [Key]
        public int LabTestId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TestCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string TestName { get; set; } = string.Empty;
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [StringLength(50)]
        public string? TestType { get; set; }
        
        public DateTime TestDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(50)]
        public string? SampleType { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        
        [Column(TypeName = "TEXT")]
        public string? Result { get; set; }
        
        [StringLength(200)]
        public string? NormalRange { get; set; }
        
        [StringLength(50)]
        public string? Units { get; set; }
        
        [StringLength(100)]
        public string? PerformedBy { get; set; }
        
        [StringLength(100)]
        public string? VerifiedBy { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [StringLength(500)]
        public string? ReportFile { get; set; }
        
        
        [ForeignKey("PatientId")]
        public virtual HospitalManagementSystem.Data.Entities.Patient Patient { get; set; } = null!;
        
        [ForeignKey("DoctorId")]
        public virtual HospitalManagementSystem.Data.Entities.Doctor Doctor { get; set; } = null!;
    }
}