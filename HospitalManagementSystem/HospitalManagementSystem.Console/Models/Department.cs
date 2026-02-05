namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? HeadDoctorName { get; set; }
        public int? HeadDoctorId { get; set; }
        public int StaffCount { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
