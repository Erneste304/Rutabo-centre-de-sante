namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class DashboardStatsModel
    {
        public int TotalPatients { get; set; }
        public int TotalStaff { get; set; }
        public decimal ClinicRevenue { get; set; }
        public int AvgWaitTime { get; set; }
        public int ActiveShifts { get; set; }
        public int TotalDepartments { get; set; }
        public int SatisfactionRate { get; set; }
        public int ClaimsProcessed { get; set; }
        public double ResourceUtilization { get; set; }
        public object? RoleStats { get; set; }
    }
}
