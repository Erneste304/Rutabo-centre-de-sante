using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    /// <summary>
    /// Many-to-Many join table between MedicalRecords and Medicine
    /// as shown in the ER diagram (Medical_Records_Medicine).
    /// </summary>
    [Table("Medical_Records_Medicine")]
    public class MedicalRecordMedicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RecordId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [StringLength(100)]
        public string? Dosage { get; set; }

        // Navigation properties
        [ForeignKey("RecordId")]
        public virtual HospitalManagementSystem.Data.Entities.MedicalRecord MedicalRecord { get; set; } = null!;

        [ForeignKey("MedicineId")]
        public virtual HospitalManagementSystem.Data.Entities.Medicine Medicine { get; set; } = null!;
    }
}
