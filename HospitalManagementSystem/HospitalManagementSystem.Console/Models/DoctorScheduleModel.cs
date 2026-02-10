using System;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class DoctorScheduleModel
    {
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Monday { get; set; } = "Off";
        public string Tuesday { get; set; } = "Off";
        public string Wednesday { get; set; } = "Off";
        public string Thursday { get; set; } = "Off";
        public string Friday { get; set; } = "Off";
        public string Saturday { get; set; } = "Off";
        public string Sunday { get; set; } = "Off";
    }
}
