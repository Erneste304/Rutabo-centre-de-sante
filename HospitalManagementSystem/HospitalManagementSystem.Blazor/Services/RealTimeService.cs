using Microsoft.AspNetCore.SignalR.Client;

namespace HospitalManagementSystem.Blazor.Services
{
    /// <summary>
    /// Manages the SignalR connection for real-time hospital updates.
    /// Provides events for bed status, emergency alerts, and queue changes.
    /// </summary>
    public class RealTimeService : IAsyncDisposable
    {
        private HubConnection? _hubConnection;
        private readonly string _hubUrl;

        public event Action<object>? OnBedStatusUpdated;
        public event Action<EmergencyAlertDto>? OnEmergencyAlert;
        public event Action<object>? OnQueueUpdated;
        public event Action<bool>? OnConnectionChanged;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public RealTimeService(HttpClient httpClient)
        {
            // Derive hub URL from the API base address
            var baseUrl = httpClient.BaseAddress?.ToString().TrimEnd('/') ?? "http://localhost:5051";
            _hubUrl = $"{baseUrl}/hubs/hospital";
        }

        public async Task StartAsync()
        {
            if (_hubConnection != null) return;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<object>("BedStatusUpdated", data =>
            {
                OnBedStatusUpdated?.Invoke(data);
            });

            _hubConnection.On<EmergencyAlertDto>("EmergencyAlert", alert =>
            {
                OnEmergencyAlert?.Invoke(alert);
            });

            _hubConnection.On<object>("QueueUpdated", data =>
            {
                OnQueueUpdated?.Invoke(data);
            });

            _hubConnection.Reconnected += _ =>
            {
                OnConnectionChanged?.Invoke(true);
                return Task.CompletedTask;
            };

            _hubConnection.Closed += _ =>
            {
                OnConnectionChanged?.Invoke(false);
                return Task.CompletedTask;
            };

            try
            {
                await _hubConnection.StartAsync();
                OnConnectionChanged?.Invoke(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR connection failed: {ex.Message}");
                OnConnectionChanged?.Invoke(false);
            }
        }

        public async Task JoinGroupAsync(string groupName)
        {
            if (IsConnected)
                await _hubConnection!.InvokeAsync("JoinGroup", groupName);
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }

    public class EmergencyAlertDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public DateTime Timestamp { get; set; }
    }
}
