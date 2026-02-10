using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("PrescriptionItems")]
    public class PrescriptionItem
    {
        [Key]
        public int PrescriptionItemId { get; set; }
        
        [Required]
        public int PrescriptionId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string MedicationName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Dosage { get; set; }
        
        [StringLength(50)]
        public string? Frequency { get; set; }
        
        [StringLength(50)]
        public string? Duration { get; set; }
        
        public int? Quantity { get; set; }
        
        [StringLength(500)]
        public string? Instructions { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Prescribed";
        
        
        [ForeignKey("PrescriptionId")]
        public virtual HospitalManagementSystem.Data.Entities.Prescription Prescription { get; set; } = null!;
    }
}