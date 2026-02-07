using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Billing")]
    public class Billing
    {
        [Key]
        public int BillId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string BillNumber { get; set; } = string.Empty;
        
        [Required]
        public int PatientId { get; set; }
        
        public int? AppointmentId { get; set; }
        
        public DateTime BillDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? DueDate { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxAmount { get; set; } = 0;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; } = 0;
        
        [Column(TypeName = "decimal(12,2)")]
        public decimal PaidAmount { get; set; } = 0;
        
        [Column(TypeName = "decimal(12,2)")]
        public decimal BalanceDue { get; set; } = 0;
        
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Pending"; 
        
        [StringLength(100)]
        public string? InsuranceProvider { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal InsuranceCoverage { get; set; } = 0;
        
        [StringLength(50)]
        public string? InsuranceClaimNumber { get; set; }
        
        [StringLength(20)]
        public string ClaimStatus { get; set; } = "Not Submitted";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int? CreatedBy { get; set; }
        
        
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;
        
        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }
        
        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }
        
        public virtual ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}