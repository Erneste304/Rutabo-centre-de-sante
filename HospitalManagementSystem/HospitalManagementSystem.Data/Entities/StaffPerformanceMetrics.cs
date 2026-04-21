using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    /// <summary>
    /// Represents staff performance metrics for analytics
    /// </summary>
    [Table("StaffPerformanceMetrics")]
    public class StaffPerformanceMetrics
    {
        [Key]
        public int MetricId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [StringLength(50)]
        public string MetricType { get; set; } // Appointments, PatientSatisfaction, AverageWaitTime, etc.

        public decimal MetricValue { get; set; }

        public decimal? TargetValue { get; set; }

        public decimal? PercentageOfTarget { get; set; }

        public DateTime DateRecorded { get; set; } = DateTime.Now;

        public DateTime? DateUpdated { get; set; }

        [StringLength(50)]
        public string Period { get; set; } // Daily, Weekly, Monthly, Quarterly, Yearly

        public int? AppointmentsCompleted { get; set; }

        public int? PatientsServed { get; set; }

        public decimal? AveragePatientSatisfactionScore { get; set; } // 1-5

        public decimal? AverageWaitTimeMinutes { get; set; }

        public int? NoShowCount { get; set; }

        public int? CancellationCount { get; set; }

        public decimal? OnTimePercentage { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public bool IsAboveTarget { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Status { get; set; } // Excellent, Good, Average, NeedImprovement
    }
}
