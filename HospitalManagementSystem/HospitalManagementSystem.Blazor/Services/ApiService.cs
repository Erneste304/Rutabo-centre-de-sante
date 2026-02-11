using System.Net.Http.Json;
using HospitalManagementSystem.Blazor.Models.DTOs;

namespace HospitalManagementSystem.Blazor.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<T?> RequestAsync<T>(string endpoint, HttpMethod? method = null, object? data = null)
        {
            try 
            {
                var request = new HttpRequestMessage(method ?? HttpMethod.Get, endpoint);
                
                if (data != null)
                {
                    request.Content = JsonContent.Create(data);
                }
                
                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch
            {
                return default;
            }
        }
        
        public async Task<UserModel?> LoginAsync(string username, string password)
        {
            // Adjust endpoint as needed. Using a common convention.
            return await RequestAsync<UserModel>("api/auth/login", HttpMethod.Post, new 
            { 
                Username = username, 
                Password = password 
            });
        }
    }
}
