using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Insurance_Types")]
    public class InsuranceType
    {
        [Key]
        public int InsuranceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // Mutuelle, RAMA, etc.

        [Required]
        [StringLength(20)]
        public string Code { get; set; } = string.Empty; // MUTUELLE, RAMA

        [Column(TypeName = "decimal(5,2)")]
        public decimal CoveragePercentage { get; set; } // 90 for 90%, 100 for 100%
        
        public bool IsActive { get; set; } = true;
    }
}
