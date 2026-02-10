using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        public int LogId { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        public int? UserId { get; set; }
        
        [StringLength(50)]
        public string? UserRole { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? EntityType { get; set; }
        
        public int? EntityId { get; set; }
        
        [Column(TypeName = "TEXT")]
        public string? Details { get; set; }
        
        [StringLength(45)]
        public string? IPAddress { get; set; }
        
        [StringLength(500)]
        public string? UserAgent { get; set; }
        
        [Column(TypeName = "TEXT")]
        public string? Changes { get; set; }
        
        
        [ForeignKey("UserId")]
        public virtual HospitalManagementSystem.Data.Entities.User? User { get; set; }
    }
}