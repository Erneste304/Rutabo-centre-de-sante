namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class ShiftReportModel
    {
        public int ReportId { get; set; }
        public DateTime ShiftDate { get; set; } = DateTime.Today;
        public string ShiftType { get; set; } = "Morning";
        public string StartTime { get; set; } = "07:00";
        public string EndTime { get; set; } = "15:00";
        public string? PatientsHandled { get; set; }
        public string? TasksCompleted { get; set; }
        public string? MedicationsAdministered { get; set; }
        public string? CriticalIncidents { get; set; }
        public string? IssuesConcerns { get; set; }
        public string? HandoverNotes { get; set; }
        public string? NextShiftNotes { get; set; }
        public bool IsCompleted { get; set; }
    }
}