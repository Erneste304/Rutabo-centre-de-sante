using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class InventoryModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string Category { get; set; } = "Medication";
        public string? Description { get; set; }
        public string? Unit { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Supplier { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; } = "Active";
    }

    public class MedicineModel
    {
        public int MedicineId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public string? Dosage { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class PrescriptionModel
    {
        public int PrescriptionId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime PrescriptionDate { get; set; }
        public string? Diagnosis { get; set; }
        public string? Instructions { get; set; }
        public string Status { get; set; } = "Active";
        public List<PrescriptionItemModel> Items { get; set; } = new();
    }

    public class PrescriptionItemModel
    {
        public int ItemId { get; set; }
        public int PrescriptionId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Instructions { get; set; }
    }
}
