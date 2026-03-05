using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string RoomNumber { get; set; } = string.Empty;
        
        /// <summary>FK to RoomType table (replaces plain string RoomType)</summary>
        public int? RoomTypeId { get; set; }
        
        public int? DepartmentId { get; set; }
        
        public int? FloorNumber { get; set; }
        
        public int BedCount { get; set; } = 1;
        
        public int AvailableBeds { get; set; } = 1;
        
        [StringLength(20)]
        public string RoomStatus { get; set; } = "Available"; // Available, Occupied, Cleaning, Maintenance, Reserved
        
        [StringLength(500)]
        public string? Equipment { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal DailyRate { get; set; } = 0;
        
        [StringLength(500)]
        public string? Features { get; set; }
        
        [StringLength(100)]
        public string? PatientName { get; set; }
        
        public DateTime? OccupiedSince { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        
        [ForeignKey("DepartmentId")]
        public virtual HospitalManagementSystem.Data.Entities.Department? Department { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual HospitalManagementSystem.Data.Entities.RoomType? RoomType { get; set; }

        public virtual ICollection<HospitalManagementSystem.Data.Entities.RoomAssignment> RoomAssignments { get; set; } = new List<HospitalManagementSystem.Data.Entities.RoomAssignment>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.CleaningService> CleaningServices { get; set; } = new List<HospitalManagementSystem.Data.Entities.CleaningService>();
    }
}