using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Triage_Records")]
    public class TriageRecord
    {
        [Key]
        public int TriageId { get; set; }

        [Required]
        public int VisitId { get; set; }

        [Column(TypeName = "decimal(4,1)")]
        public decimal? Temperature { get; set; }

        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public int? RespiratoryRate { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        [StringLength(1000)]
        public string? Notes { get; set; }

        [JsonIgnore]
        [ForeignKey("VisitId")]
        public virtual PatientVisit Visit { get; set; } = null!;
    }
}
