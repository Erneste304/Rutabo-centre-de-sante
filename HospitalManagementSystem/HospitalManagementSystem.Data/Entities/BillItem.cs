using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("BillItems")]
    public class BillItem
    {
        [Key]
        public int ItemId { get; set; }
        
        [Required]
        public int BillId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ItemType { get; set; } = string.Empty; 
        
        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        
        public int Quantity { get; set; } = 1;
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [ForeignKey("BillId")]
        public virtual Billing Billing { get; set; } = null!;
    }
}