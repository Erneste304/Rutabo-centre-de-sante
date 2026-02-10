using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string DepartmentCode { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public int? HeadDoctorId { get; set; }
        
        [StringLength(10)]
        public string? PhoneExtension { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        public int TotalBeds { get; set; } = 0;
        
        public int AvailableBeds { get; set; } = 0;
        
        [StringLength(20)]
        public string Status { get; set; } = "Active"; 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        
        [ForeignKey("HeadDoctorId")]
        public virtual HospitalManagementSystem.Data.Entities.Doctor? HeadDoctor { get; set; }
        
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Room> Rooms { get; set; } = new List<HospitalManagementSystem.Data.Entities.Room>();
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Doctor> Doctors { get; set; } = new List<HospitalManagementSystem.Data.Entities.Doctor>();
    }
}