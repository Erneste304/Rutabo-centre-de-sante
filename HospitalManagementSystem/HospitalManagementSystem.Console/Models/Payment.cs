namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty; // MTN, Tigo, Virtual Card, etc.
        public string Reference { get; set; } = string.Empty;
    }
}

