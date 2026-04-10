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
            return await RequestAsync<UserModel>("api/auth/login", HttpMethod.Post, new 
            { 
                Username = username, 
                Password = password 
            });
        }

        public async Task<bool> RegisterAsync(object registerModel)
        {
            var result = await RequestAsync<object>("api/auth/register", HttpMethod.Post, registerModel);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string username)
        {
            var result = await RequestAsync<object>("api/auth/forgot-password", HttpMethod.Post, new 
            { 
                Username = username 
            });
            return true;
        }
    }
}
