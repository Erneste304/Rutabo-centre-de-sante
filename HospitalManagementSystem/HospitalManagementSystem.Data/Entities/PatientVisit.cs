using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Patient_Visits")]
    public class PatientVisit
    {
        [Key]
        public int VisitId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        [StringLength(50)]
        public string VisitNumber { get; set; } = string.Empty;

        public DateTime VisitDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string VisitType { get; set; } = "General"; // General, Emergency, Maternity

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Registered"; // Registered, InTriage, WaitingForDoctor, Completed

        [StringLength(20)]
        public string Priority { get; set; } = "Normal"; // High, Normal, Urgent

        [StringLength(500)]
        public string InitialComplaint { get; set; } = string.Empty;

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;
        
        public virtual TriageRecord? TriageRecord { get; set; }
    }
}
