using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "TEXT")]
        public string Message { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Type { get; set; } = "System"; // Appointment, Payment, System, Alert, Reminder
        
        [StringLength(20)]
        public string Priority { get; set; } = "Normal"; 
        
        public bool IsRead { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ExpiresAt { get; set; }
        
        [StringLength(500)]
        public string? ActionUrl { get; set; }
        
        
        [ForeignKey("UserId")]
        public virtual HospitalManagementSystem.Data.Entities.User User { get; set; } = null!;
    }
}