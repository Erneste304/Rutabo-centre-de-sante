using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class PatientModel
    {
        public int PatientId { get; set; }
        public string MedicalRecordNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? BloodType { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? InsurancePolicyNumber { get; set; }
    }

    public class PatientVisitModel
    {
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public string VisitNumber { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public string VisitType { get; set; } = string.Empty; // General, Emergency, Maternity
        public string Status { get; set; } = string.Empty; // Registered, InTriage, WaitingForDoctor, Completed
        public string Priority { get; set; } = string.Empty; // High, Normal
        public string InitialComplaint { get; set; } = string.Empty;
        public PatientModel? Patient { get; set; }
        public TriageRecordModel? TriageRecord { get; set; }
    }

    public class TriageRecordModel
    {
        public int TriageId { get; set; }
        public int VisitId { get; set; }
        public decimal? Temperature { get; set; }
        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? Weight { get; set; }
        public string? Notes { get; set; }
    }

    public class InsuranceTypeModel
    {
        public int InsuranceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal CoveragePercentage { get; set; }
    }
}
