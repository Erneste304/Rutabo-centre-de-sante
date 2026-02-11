using Microsoft.JSInterop;
using System.Text.Json;
using HospitalManagementSystem.Blazor.Models.DTOs;

namespace HospitalManagementSystem.Blazor.Services
{
    public class AuthService
    {
        private readonly ApiService _api;
        private readonly IJSRuntime _jsRuntime;
        private UserModel? _currentUser;
        
        public AuthService(ApiService api, IJSRuntime jsRuntime)
        {
            _api = api;
            _jsRuntime = jsRuntime;
        }
        
        public UserModel? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;

        public async Task<UserModel?> LoginAsync(string username, string password, string userType)
        {
            try
            {
                // Attempt real login
                var user = await _api.LoginAsync(username, password);
                
                if (user != null)
                {
                    _currentUser = user;
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "user", 
                        JsonSerializer.Serialize(user));
                    return user;
                }
            }
            catch
            {
                // Ignore API failure for fallback
            }
            
            // Fallback to mock login
            return await MockLogin(username, password, userType);
        }

        public async Task LogoutAsync()
        {
            _currentUser = null;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user");
        }
        
        private async Task<UserModel?> MockLogin(string username, string password, string userType)
        {
            await Task.Delay(500); // Simulate delay
            
            var mockUser = new UserModel 
            { 
                Id = new Random().Next(1, 1000), 
                Username = username, 
                FullName = string.IsNullOrEmpty(username) ? "User" : username,
                UserType = userType,
                Department = "General", 
                Status = "Active",
                CreatedAt = DateTime.Now,
                LastLogin = DateTime.Now
            };
            
            _currentUser = mockUser;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "user", 
                    JsonSerializer.Serialize(mockUser));
            return mockUser;
        }
        
        public async Task<bool> CheckAuthenticationAsync()
        {
            try 
            {
                var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "user");
                if (!string.IsNullOrEmpty(userJson))
                {
                    _currentUser = JsonSerializer.Deserialize<UserModel>(userJson);
                    return true;
                }
            }
            catch {}
            return false;
        }
        
        public async Task<UserModel?> GetCurrentUserAsync()
        {
            if (_currentUser == null)
            {
                await CheckAuthenticationAsync();
            }
            return _currentUser;
        }
    }
}