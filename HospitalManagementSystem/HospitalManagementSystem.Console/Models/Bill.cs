namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty; // e.g. Pending, Paid, Cancelled
        public string Description { get; set; } = string.Empty;
    }
}

