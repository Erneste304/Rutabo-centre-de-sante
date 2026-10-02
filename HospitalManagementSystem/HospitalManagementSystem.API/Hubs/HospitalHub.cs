using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HospitalManagementSystem.API.Hubs
{
    /// <summary>
    /// SignalR hub for real-time hospital notifications.
    /// Hospital staff receive server-originated real-time notifications.
    /// </summary>
    [Authorize(Policy = "HospitalStaff")]
    public class HospitalHub : Hub
    {
    }
}
