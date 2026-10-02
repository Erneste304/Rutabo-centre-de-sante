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

        public async Task<UserModel?> LoginAsync(string username, string password)
        {
            var user = await _api.LoginAsync(username, password);
            if (user == null)
                return null;

            _currentUser = user;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "user",
                JsonSerializer.Serialize(user));
            return user;
        }

        public async Task<bool> RegisterAsync(string username, string fullName, string email, string password)
        {
            var registerModel = new
            {
                Username = username,
                FullName = fullName,
                Email = email,
                Password = password,
                UserType = 3
            };

            return await _api.RegisterAsync(registerModel);
        }

        public async Task<bool> ForgotPasswordAsync(string username)
        {
            return await _api.ForgotPasswordAsync(username);
        }

        public async Task LogoutAsync()
        {
            _currentUser = null;
            _api.ClearTokenCache();
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user");
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