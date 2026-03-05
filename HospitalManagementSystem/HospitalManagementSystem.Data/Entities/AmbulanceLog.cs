using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Ambulance_Log")]
    public class AmbulanceLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public int AmbulanceId { get; set; }

        public int? PatientId { get; set; }

        [StringLength(255)]
        public string? PickupLocation { get; set; }

        [StringLength(255)]
        public string? DropoffLocation { get; set; }

        public DateTime? PickupTime { get; set; }

        public DateTime? DropoffTime { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled

        // Navigation properties
        [ForeignKey("AmbulanceId")]
        public virtual HospitalManagementSystem.Data.Entities.Ambulance Ambulance { get; set; } = null!;

        [ForeignKey("PatientId")]
        public virtual HospitalManagementSystem.Data.Entities.Patient? Patient { get; set; }
    }
}
