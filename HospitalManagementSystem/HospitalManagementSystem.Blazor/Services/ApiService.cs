using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HospitalManagementSystem.Blazor.Models.DTOs;
using Microsoft.JSInterop;

namespace HospitalManagementSystem.Blazor.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private string? _cachedToken;

        public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        private async Task AttachTokenAsync(HttpRequestMessage request)
        {
            try
            {
                if (_cachedToken == null)
                {
                    var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "user");
                    if (!string.IsNullOrEmpty(userJson))
                    {
                        var user = JsonSerializer.Deserialize<UserModel>(userJson,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        _cachedToken = user?.Token;
                    }
                }

                if (!string.IsNullOrEmpty(_cachedToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken);
                }
            }
            catch
            {
                // Ignore token attachment errors
            }
        }

        public void ClearTokenCache() => _cachedToken = null;

        public async Task<T?> RequestAsync<T>(string endpoint, HttpMethod? method = null, object? data = null)
        {
            try
            {
                var request = new HttpRequestMessage(method ?? HttpMethod.Get, endpoint);
                await AttachTokenAsync(request);

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

                return await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return default;
            }
        }

        public async Task<UserModel?> LoginAsync(string username, string password)
        {
            // Login doesn't need a token
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login");
                request.Content = JsonContent.Create(new { Username = username, Password = password });
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;
                var user = await response.Content.ReadFromJsonAsync<UserModel>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (user?.Token != null)
                    _cachedToken = user.Token;
                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> RegisterAsync(object registerModel)
        {
            var result = await RequestAsync<object>("api/auth/register", HttpMethod.Post, registerModel);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string username)
        {
            await RequestAsync<object>("api/auth/forgot-password", HttpMethod.Post, new { Username = username });
            return true;
        }

        // Typed helpers
        public async Task<T?> GetAsync<T>(string endpoint) =>
            await RequestAsync<T>(endpoint, HttpMethod.Get);

        public async Task<List<object>> GetAsync(string endpoint) =>
            await RequestAsync<List<object>>(endpoint, HttpMethod.Get) ?? new List<object>();

        public async Task<T?> PostAsync<T>(string endpoint, object data) =>
            await RequestAsync<T>(endpoint, HttpMethod.Post, data);

        public async Task<object?> PostAsync(string endpoint, object? data) =>
            await RequestAsync<object>(endpoint, HttpMethod.Post, data);

        public async Task<T?> PutAsync<T>(string endpoint, object data) =>
            await RequestAsync<T>(endpoint, HttpMethod.Put, data);

        public async Task<object?> PutAsync(string endpoint, object data) =>
            await RequestAsync<object>(endpoint, HttpMethod.Put, data);

        public async Task<T?> DeleteAsync<T>(string endpoint) =>
            await RequestAsync<T>(endpoint, HttpMethod.Delete);

        public async Task<object?> DeleteAsync(string endpoint) =>
            await RequestAsync<object>(endpoint, HttpMethod.Delete);

        public async Task<bool> PatchAsync(string endpoint, object data)
        {
            var result = await RequestAsync<object>(endpoint, HttpMethod.Patch, data);
            return true;
        }
    }
}
