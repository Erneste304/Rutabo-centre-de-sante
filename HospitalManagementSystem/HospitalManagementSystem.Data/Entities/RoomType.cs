using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Data.Entities
{
    [Table("Room_Types")]
    public class RoomType
    {
        [Key]
        public int RoomTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomTypeName { get; set; } = string.Empty; // General, ICU, Emergency, Private, etc.

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<HospitalManagementSystem.Data.Entities.Room> Rooms { get; set; } = new List<HospitalManagementSystem.Data.Entities.Room>();
    }
}
