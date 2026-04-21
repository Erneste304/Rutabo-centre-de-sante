using Microsoft.AspNetCore.SignalR;

namespace HospitalManagementSystem.API.Hubs
{
    /// <summary>
    /// SignalR hub for real-time hospital notifications.
    /// Clients can join groups by role or department.
    /// </summary>
    public class HospitalHub : Hub
    {
        // Called when a client connects
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        // Join a named group (e.g. "Admin", "Nurse", "Emergency")
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Leave a group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        // Broadcast a bed status update to all clients
        public async Task BedStatusUpdated(object bedData)
        {
            await Clients.All.SendAsync("BedStatusUpdated", bedData);
        }

        // Broadcast an emergency alert to all clients
        public async Task EmergencyAlert(object alertData)
        {
            await Clients.All.SendAsync("EmergencyAlert", alertData);
        }

        // Broadcast a queue update
        public async Task QueueUpdated(object queueData)
        {
            await Clients.All.SendAsync("QueueUpdated", queueData);
        }
    }
}
