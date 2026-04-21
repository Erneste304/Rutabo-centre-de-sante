using HospitalManagementSystem.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HospitalManagementSystem.API.Services
{
    public interface INotificationService
    {
        Task SendBedUpdateAsync(object bedData);
        Task SendEmergencyAlertAsync(string title, string message, string priority);
        Task SendQueueUpdateAsync(object queueData);
        Task SendToGroupAsync(string group, string method, object data);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<HospitalHub> _hubContext;

        public NotificationService(IHubContext<HospitalHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendBedUpdateAsync(object bedData)
        {
            await _hubContext.Clients.All.SendAsync("BedStatusUpdated", bedData);
        }

        public async Task SendEmergencyAlertAsync(string title, string message, string priority)
        {
            var alert = new
            {
                Title = title,
                Message = message,
                Priority = priority,
                Timestamp = DateTime.UtcNow
            };
            await _hubContext.Clients.All.SendAsync("EmergencyAlert", alert);
        }

        public async Task SendQueueUpdateAsync(object queueData)
        {
            await _hubContext.Clients.All.SendAsync("QueueUpdated", queueData);
        }

        public async Task SendToGroupAsync(string group, string method, object data)
        {
            await _hubContext.Clients.Group(group).SendAsync(method, data);
        }
    }
}
