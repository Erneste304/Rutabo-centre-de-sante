using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Room_Assignments")]
    public class RoomAssignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        public int RoomId { get; set; }

        /// <summary>Staff member responsible - maps to User.UserId</summary>
        public int? StaffId { get; set; }

        public int? PatientId { get; set; }

        public DateTime AssignmentDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        // Navigation properties
        [ForeignKey("RoomId")]
        public virtual HospitalManagementSystem.Data.Entities.Room Room { get; set; } = null!;

        [ForeignKey("StaffId")]
        public virtual HospitalManagementSystem.Data.Entities.User? Staff { get; set; }

        [ForeignKey("PatientId")]
        public virtual HospitalManagementSystem.Data.Entities.Patient? Patient { get; set; }
    }
}
