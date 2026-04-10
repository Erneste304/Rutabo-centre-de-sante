using System;

namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class AmbulanceModel
    {
        public int AmbulanceId { get; set; }
        public string AmbulanceNumber { get; set; } = string.Empty;
        public string Availability { get; set; } = "Available";
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public DateTime? LastServiceDate { get; set; }
    }

    public class AmbulanceLogModel
    {
        public int LogId { get; set; }
        public int AmbulanceId { get; set; }
        public string? AmbulanceNumber { get; set; }
        public int? PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? PickupLocation { get; set; }
        public string? DropoffLocation { get; set; }
        public DateTime? PickupTime { get; set; }
        public DateTime? DropoffTime { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
