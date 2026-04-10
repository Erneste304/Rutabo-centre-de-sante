using HospitalManagementSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        Task<IEnumerable<Inventory>> GetLowStockItemsAsync();
        Task<Inventory?> GetItemByIdAsync(int id);
        Task<Inventory> AddItemAsync(Inventory item);
        Task<bool> UpdateStockAsync(int id, int quantity);
        Task<IEnumerable<Medicine>> GetAllMedicinesAsync();
    }

    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetActivePrescriptionsAsync();
        Task<Prescription?> GetPrescriptionByIdAsync(int id);
        Task<Prescription> CreatePrescriptionAsync(Prescription prescription);
        Task<bool> DispensePrescriptionAsync(int id);
        Task<IEnumerable<Prescription>> GetPatientPrescriptionsAsync(int patientId);
    }
}
