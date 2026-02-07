using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        public int ItemId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ItemCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Category { get; set; } = "Medication"; 
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(20)]
        public string? Unit { get; set; }
        
        public int CurrentStock { get; set; } = 0;
        
        public int MinimumStock { get; set; } = 10;
        
        public int MaximumStock { get; set; } = 1000;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; } = 0;
        
        [StringLength(100)]
        public string? Supplier { get; set; }
        
        [StringLength(100)]
        public string? SupplierContact { get; set; }
        
        public DateTime? LastRestocked { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        
        [StringLength(100)]
        public string? StorageLocation { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Active"; 
        
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}