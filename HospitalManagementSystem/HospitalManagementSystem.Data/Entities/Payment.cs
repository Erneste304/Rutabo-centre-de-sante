using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PaymentNumber { get; set; } = string.Empty;
        
        [Required]
        public int BillId { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Credit Card, Debit Card, Insurance, Bank Transfer, Cheque, Online
        
        [StringLength(100)]
        public string? TransactionId { get; set; }
        
        [StringLength(4)]
        public string? CardLastFour { get; set; }
        
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Completed"; // Completed, Pending, Failed, Refunded
        
        [StringLength(100)]
        public string? ReferenceNumber { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public int? ReceivedBy { get; set; }
        
        
        [ForeignKey("BillId")]
        public virtual Billing Billing { get; set; } = null!;
        
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;
        
        [ForeignKey("ReceivedBy")]
        public virtual User? ReceivedByUser { get; set; }
    }
}