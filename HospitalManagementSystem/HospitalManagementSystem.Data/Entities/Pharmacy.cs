using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Pharmacy")]
    public class Pharmacy
    {
        [Key]
        public int PharmacyId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        public int? PatientId { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime PrescriptionDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("MedicineId")]
        public virtual HospitalManagementSystem.Data.Entities.Medicine Medicine { get; set; } = null!;

        [ForeignKey("PatientId")]
        public virtual HospitalManagementSystem.Data.Entities.Patient? Patient { get; set; }
    }
}
