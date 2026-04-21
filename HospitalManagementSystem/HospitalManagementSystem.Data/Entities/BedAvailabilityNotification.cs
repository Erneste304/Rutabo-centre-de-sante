using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    /// <summary>
    /// Represents real-time bed availability notifications
    /// </summary>
    [Table("BedAvailabilityNotifications")]
    public class BedAvailabilityNotification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        [Required]
        [StringLength(50)]
        public string EventType { get; set; } // BedAvailable, BedOccupied, RoomFull, RoomEmpty

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Pending, Sent, Acknowledged

        public int? AvailableBeds { get; set; }

        public int? OccupiedBeds { get; set; }

        public int? TotalCapacity { get; set; }

        public int? OccupancyPercentage { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? SentAt { get; set; }

        public DateTime? AcknowledgedAt { get; set; }

        public int? AcknowledgedByUserId { get; set; }

        [ForeignKey("AcknowledgedByUserId")]
        public virtual User AcknowledgedByUser { get; set; }

        public bool IsUrgent { get; set; } = false;

        [StringLength(100)]
        public string Department { get; set; }
    }
}
