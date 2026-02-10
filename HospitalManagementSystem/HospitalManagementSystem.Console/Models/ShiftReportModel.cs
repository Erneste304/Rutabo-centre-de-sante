using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class ShiftReportModel
    {
        public string NurseName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public List<string> PatientShortSummaries { get; set; } = new();
        public int MedsAdministered { get; set; }
        public int VitalsRecorded { get; set; }
        public int TasksCompleted { get; set; }
        public List<string> Issues { get; set; } = new();
    }
}
