using System;
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
        
        [Required]
        [StringLength(50)]
        public string RoomType { get; set; } = "General"; // General, ICU, Emergency, Operation, Private, Semi-Private, Pediatric, Maternity
        
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
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        
        [ForeignKey("DepartmentId")]
        public virtual HospitalManagementSystem.Data.Entities.Department? Department { get; set; }
    }
}