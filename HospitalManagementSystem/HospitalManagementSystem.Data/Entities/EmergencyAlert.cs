using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    /// <summary>
    /// Represents an emergency alert for critical patients
    /// </summary>
    [Table("EmergencyAlerts")]
    public class EmergencyAlert
    {
        [Key]
        public int AlertId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [Required]
        [StringLength(50)]
        public string AlertType { get; set; } // Critical, Severe, Moderate, Mild

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Active, Acknowledged, Resolved

        public int? AcknowledgedByUserId { get; set; }

        [ForeignKey("AcknowledgedByUserId")]
        public virtual User AcknowledgedByUser { get; set; }

        public DateTime? AcknowledgedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ResolvedAt { get; set; }

        [StringLength(500)]
        public string ResolutionNotes { get; set; }

        public int? Priority { get; set; } // 1-5, where 1 is highest priority

        [StringLength(100)]
        public string Department { get; set; }

        public bool NotificationSent { get; set; } = false;

        public DateTime? NotificationSentAt { get; set; }
    }
}
