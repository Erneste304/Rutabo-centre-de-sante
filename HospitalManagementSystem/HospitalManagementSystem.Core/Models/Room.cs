namespace HospitalManagementSystem.Core.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public bool IsOccupied { get; set; }
    }
}
