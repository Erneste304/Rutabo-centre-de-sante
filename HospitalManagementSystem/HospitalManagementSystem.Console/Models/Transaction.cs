using System;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int PaymentId { get; set; }
        public int PatientId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty; // Payment, Refund, Transfer
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
        public DateTime Date { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? Notes { get; set; }
    }
}
