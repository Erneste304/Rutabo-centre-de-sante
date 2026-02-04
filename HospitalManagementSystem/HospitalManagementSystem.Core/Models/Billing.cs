namespace HospitalManagementSystem.Core.Models
{
    public class Billing
    {
        public int BillingId { get; set; }
        public int PatientId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
