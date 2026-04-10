using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class MedicalRecordModel
    {
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int? AppointmentId { get; set; }
        public DateTime VisitDate { get; set; }
        public string RecordType { get; set; } = "Consultation";
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Medications { get; set; }
        public string? Dosage { get; set; }
        public string? Duration { get; set; }
        public string? LabResults { get; set; }
        public string? VitalSigns { get; set; }
        public string? Notes { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public bool IsCritical { get; set; }
    }

    public class LabTestModel
    {
        public int LabTestId { get; set; }
        public string TestCode { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? TestType { get; set; }
        public DateTime TestDate { get; set; }
        public string? SampleType { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Result { get; set; }
        public string? NormalRange { get; set; }
        public string? Units { get; set; }
        public string? PerformedBy { get; set; }
        public string? VerifiedBy { get; set; }
        public string? Notes { get; set; }
        public string? ReportFile { get; set; }
    }
}
