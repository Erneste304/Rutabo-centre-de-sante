using HospitalManagementSystem.Blazor.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Blazor.Services
{
    public class InventoryService
    {
        private readonly ApiService _api;

        public InventoryService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<InventoryModel>> GetInventoryAsync()
        {
            return await _api.RequestAsync<List<InventoryModel>>("api/inventory") ?? new List<InventoryModel>();
        }

        public async Task<List<MedicineModel>> GetMedicinesAsync()
        {
            return await _api.RequestAsync<List<MedicineModel>>("api/pharmacy/medicines") ?? new List<MedicineModel>();
        }
    }

    public class PharmacyService
    {
        private readonly ApiService _api;

        public PharmacyService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<PrescriptionModel>> GetActivePrescriptionsAsync()
        {
            return await _api.RequestAsync<List<PrescriptionModel>>("api/pharmacy/prescriptions") ?? new List<PrescriptionModel>();
        }

        public async Task<bool> DispensePrescriptionAsync(int prescriptionId)
        {
            var result = await _api.RequestAsync<object>($"api/pharmacy/prescriptions/{prescriptionId}/dispense", HttpMethod.Post, null);
            return result != null;
        }
    }
}
