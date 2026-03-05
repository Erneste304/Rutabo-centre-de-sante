using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Ambulance")]
    public class Ambulance
    {
        [Key]
        public int AmbulanceId { get; set; }

        [Required]
        [StringLength(20)]
        public string AmbulanceNumber { get; set; } = string.Empty;

        [StringLength(20)]
        public string Availability { get; set; } = "Available"; // Available, Dispatched, Maintenance

        /// <summary>Driver - maps to User.UserId where UserType = 'Driver'</summary>
        public int? DriverId { get; set; }

        public DateTime? LastServiceDate { get; set; }

        // Navigation properties
        [ForeignKey("DriverId")]
        public virtual HospitalManagementSystem.Data.Entities.User? Driver { get; set; }

        public virtual ICollection<HospitalManagementSystem.Data.Entities.AmbulanceLog> AmbulanceLogs { get; set; } = new List<HospitalManagementSystem.Data.Entities.AmbulanceLog>();
    }
}
