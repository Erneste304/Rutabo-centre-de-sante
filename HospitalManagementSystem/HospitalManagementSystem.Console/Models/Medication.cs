namespace HospitalManagementSystem.Console.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}