using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    /// <summary>
    /// Many-to-Many join table between Doctors and Departments
    /// as shown in the ER diagram (Doctor_Department).
    /// </summary>
    [Table("Doctor_Department")]
    public class DoctorDepartment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        // Navigation properties
        [ForeignKey("DoctorId")]
        public virtual HospitalManagementSystem.Data.Entities.Doctor Doctor { get; set; } = null!;

        [ForeignKey("DepartmentId")]
        public virtual HospitalManagementSystem.Data.Entities.Department Department { get; set; } = null!;
    }
}
