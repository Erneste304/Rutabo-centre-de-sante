namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class DashboardStatsModel
    {
        public int TotalPatients { get; set; }
        public int TotalStaff { get; set; }
        public decimal ClinicRevenue { get; set; }
        public int AvgWaitTime { get; set; }
        public object? RoleStats { get; set; }
    }
}
