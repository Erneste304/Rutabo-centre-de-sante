using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Cleaning_Service")]
    public class CleaningService
    {
        [Key]
        public int ServiceId { get; set; }

        [Required]
        public int RoomId { get; set; }

        public DateTime ServiceDate { get; set; } = DateTime.UtcNow;

        public TimeSpan? ServiceTime { get; set; }

        /// <summary>Cleaning staff - maps to User.UserId</summary>
        public int? StaffId { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("RoomId")]
        public virtual HospitalManagementSystem.Data.Entities.Room Room { get; set; } = null!;

        [ForeignKey("StaffId")]
        public virtual HospitalManagementSystem.Data.Entities.User? Staff { get; set; }
    }
}
