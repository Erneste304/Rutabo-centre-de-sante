using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Blood_Bank")]
    public class BloodBank
    {
        [Key]
        public int BloodId { get; set; }

        [Required]
        [StringLength(5)]
        public string BloodType { get; set; } = string.Empty; // A+, A-, B+, B-, O+, O-, AB+, AB-

        public int StockQuantity { get; set; } = 0;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
