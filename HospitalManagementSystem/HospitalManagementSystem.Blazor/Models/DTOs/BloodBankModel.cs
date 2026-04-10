using System;

namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class BloodBankModel
    {
        public int BloodId { get; set; }
        public string BloodType { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
