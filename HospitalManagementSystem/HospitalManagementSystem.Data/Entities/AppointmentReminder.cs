using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    /// <summary>
    /// Represents appointment reminders for patients
    /// </summary>
    [Table("AppointmentReminders")]
    public class AppointmentReminder
    {
        [Key]
        public int ReminderId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [Required]
        [StringLength(50)]
        public string ReminderType { get; set; } // Email, SMS, InApp

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Pending, Sent, Failed, Acknowledged

        public DateTime ScheduledTime { get; set; }

        public DateTime? SentTime { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public int? HoursBeforeAppointment { get; set; } // 24, 12, 2, etc.

        public bool IsAcknowledged { get; set; } = false;

        public DateTime? AcknowledgedTime { get; set; }

        [StringLength(500)]
        public string ErrorMessage { get; set; }

        public int? RetryCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
