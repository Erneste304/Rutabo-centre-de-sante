using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagementSystem.Console.Models;

namespace HospitalManagementSystem.Console.Services
{
    public interface IDataService
    {
        Task<List<Patient>> GetPatientsAsync();
        Task<List<Doctor>> GetDoctorsAsync();
        Task<List<Appointment>> GetAppointmentsAsync();
        Task<List<Room>> GetRoomsAsync();
        Task<List<Medication>> GetMedicationsAsync();
        Task<bool> AddPatientAsync(Patient patient);
        Task<bool> UpdatePatientAsync(Patient patient);
        Task<bool> ScheduleAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<DashboardStats> GetDashboardStatsAsync(string userType);
    }

    public class DataService : IDataService
    {
        // Mock data - In real app, connect to API or database
        public async Task<List<Patient>> GetPatientsAsync()
        {
            await Task.Delay(50);
            return new List<Patient>
            {
                new Patient { PatientId = 1001, Name = "John Doe", Age = 45, Gender = "Male", BloodType = "O+", LastVisit = DateTime.Now.AddDays(-10) },
                new Patient { PatientId = 1002, Name = "Emily Davis", Age = 32, Gender = "Female", BloodType = "A-", LastVisit = DateTime.Now.AddDays(-5) },
                new Patient { PatientId = 1003, Name = "Robert Johnson", Age = 58, Gender = "Male", BloodType = "B+", LastVisit = DateTime.Now.AddDays(-2) },
                new Patient { PatientId = 1004, Name = "Sarah Wilson", Age = 28, Gender = "Female", BloodType = "AB+", LastVisit = DateTime.Now.AddDays(-15) }
            };
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            await Task.Delay(50);
            return new List<Doctor>
            {
                new Doctor { DoctorId = 101, Name = "Dr. John Smith", Specialization = "Cardiology", Department = "Cardiology", IsAvailable = true },
                new Doctor { DoctorId = 102, Name = "Dr. Sarah Jones", Specialization = "Neurology", Department = "Neurology", IsAvailable = true },
                new Doctor { DoctorId = 103, Name = "Dr. Michael Brown", Specialization = "Orthopedics", Department = "Orthopedics", IsAvailable = false },
                new Doctor { DoctorId = 104, Name = "Dr. Lisa Wang", Specialization = "Pediatrics", Department = "Pediatrics", IsAvailable = true }
            };
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            await Task.Delay(50);
            return new List<Appointment>
            {
                new Appointment { AppointmentId = 1, PatientName = "John Doe", DoctorName = "Dr. John Smith", Date = DateTime.Now.AddHours(2), Status = "Scheduled" },
                new Appointment { AppointmentId = 2, PatientName = "Emily Davis", DoctorName = "Dr. Sarah Jones", Date = DateTime.Now.AddDays(1), Status = "Confirmed" },
                new Appointment { AppointmentId = 3, PatientName = "Robert Johnson", DoctorName = "Dr. Michael Brown", Date = DateTime.Now.AddDays(-1), Status = "Completed" }
            };
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            await Task.Delay(50);
            return new List<Room>
            {
                new Room { RoomNumber = "101", Type = "General", Status = "Occupied", PatientName = "John Doe" },
                new Room { RoomNumber = "102", Type = "General", Status = "Available", PatientName = "" },
                new Room { RoomNumber = "ICU-01", Type = "ICU", Status = "Occupied", PatientName = "Emily Davis" },
                new Room { RoomNumber = "OP-01", Type = "Operation", Status = "Available", PatientName = "" }
            };
        }

        public async Task<List<Medication>> GetMedicationsAsync()
        {
            await Task.Delay(50);
            return new List<Medication>
            {
                new Medication { Id = 1, Name = "Amoxicillin", Type = "Antibiotic", Stock = 150, Unit = "mg" },
                new Medication { Id = 2, Name = "Ibuprofen", Type = "Pain Reliever", Stock = 200, Unit = "mg" },
                new Medication { Id = 3, Name = "Lisinopril", Type = "Blood Pressure", Stock = 100, Unit = "mg" },
                new Medication { Id = 4, Name = "Metformin", Type = "Diabetes", Stock = 120, Unit = "mg" }
            };
        }

        public async Task<bool> AddPatientAsync(Patient patient)
        {
            await Task.Delay(100);
            Console.WriteLine($"Patient {patient.Name} added successfully!");
            return true;
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            await Task.Delay(100);
            Console.WriteLine($"Patient {patient.Name} updated successfully!");
            return true;
        }

        public async Task<bool> ScheduleAppointmentAsync(Appointment appointment)
        {
            await Task.Delay(100);
            Console.WriteLine($"Appointment scheduled for {appointment.PatientName} with {appointment.DoctorName}");
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            await Task.Delay(100);
            Console.WriteLine($"Appointment {appointment.AppointmentId} updated successfully!");
            return true;
        }

        public async Task<DashboardStats> GetDashboardStatsAsync(string userType)
        {
            await Task.Delay(100);
            var stats = new DashboardStats
            {
                Date = DateTime.Now
            };

            switch (userType.ToLower())
            {
                case "doctor":
                    stats.TotalPatients = 45;
                    stats.TodayAppointments = 8;
                    stats.PendingTasks = 3;
                    stats.Stat1Label = "Monthly Consultations";
                    stats.Stat1Value = "156";
                    stats.Stat2Label = "Success Rate";
                    stats.Stat2Value = "94%";
                    break;
                    
                case "nurse":
                    stats.TotalPatients = 25;
                    stats.TodayAppointments = 15;
                    stats.PendingTasks = 10;
                    stats.Stat1Label = "Medications Administered";
                    stats.Stat1Value = "89";
                    stats.Stat2Label = "Patient Satisfaction";
                    stats.Stat2Value = "96%";
                    break;
                    
                case "receptionist":
                    stats.TotalPatients = 120;
                    stats.TodayAppointments = 45;
                    stats.PendingTasks = 12;
                    stats.Stat1Label = "Calls Handled";
                    stats.Stat1Value = "156";
                    stats.Stat2Label = "Appointments Booked";
                    stats.Stat2Value = "89";
                    break;
                    
                default: // Admin
                    stats.TotalPatients = 1245;
                    stats.TodayAppointments = 156;
                    stats.PendingTasks = 23;
                    stats.Stat1Label = "Total Staff";
                    stats.Stat1Value = "145";
                    stats.Stat2Label = "Bed Occupancy";
                    stats.Stat2Value = "80%";
                    break;
            }
            
            return stats;
        }
    }
}