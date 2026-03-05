using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Medicine")]
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Brand { get; set; }

        [StringLength(50)]
        public string? Type { get; set; } // Tablet, Capsule, Syrup, Injection, etc.

        [StringLength(50)]
        public string? Dosage { get; set; }

        public int StockQuantity { get; set; } = 0;

        public DateTime? ExpiryDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Pharmacy> PharmacyEntries { get; set; } = new List<HospitalManagementSystem.Data.Entities.Pharmacy>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.MedicalRecordMedicine> MedicalRecordMedicines { get; set; } = new List<HospitalManagementSystem.Data.Entities.MedicalRecordMedicine>();
    }
}
