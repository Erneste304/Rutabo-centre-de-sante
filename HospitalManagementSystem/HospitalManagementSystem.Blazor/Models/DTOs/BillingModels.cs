using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class BillingModel
    {
        public int BillId { get; set; }
        public string BillNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string? InsuranceProvider { get; set; }
        public string ClaimStatus { get; set; } = "Not Submitted";
        public List<BillItemModel> BillItems { get; set; } = new();
        public List<PaymentModel> Payments { get; set; } = new();
    }

    public class BillItemModel
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public string PaymentNumber { get; set; } = string.Empty;
        public int BillId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public string? TransactionId { get; set; }
        public string PaymentStatus { get; set; } = "Completed";
    }
}
