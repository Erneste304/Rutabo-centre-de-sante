using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    /// <summary>
    /// Represents a patient in the queue system
    /// </summary>
    [Table("PatientQueues")]
    public class PatientQueue
    {
        [Key]
        public int QueueId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Waiting, Called, In-Progress, Completed, Cancelled

        public int? QueueNumber { get; set; }

        [Required]
        public DateTime CheckInTime { get; set; } = DateTime.Now;

        public DateTime? CalledTime { get; set; }

        public DateTime? CompletionTime { get; set; }

        public int? AssignedDoctorId { get; set; }

        [ForeignKey("AssignedDoctorId")]
        public virtual Doctor AssignedDoctor { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        public int? EstimatedWaitTimeMinutes { get; set; }

        public int? ActualWaitTimeMinutes { get; set; }

        [StringLength(50)]
        public string Priority { get; set; } // Normal, Urgent, Emergency

        public bool IsNoShow { get; set; } = false;

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
