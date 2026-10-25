using CampusNova.Api.Hubs;
using CampusNova.Domain.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Realtime.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubcontext)
        {
            _hubContext = hubcontext;
        }

        public async Task SendToAllAsync(string message)
             => await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);

        public async Task SendToUserAsync(string userId, string message)
            => await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);

        public async Task SendToGroupAsync(string group, string message)
            => await _hubContext.Clients.Group(group).SendAsync("ReceiveNotification", message);
    }
}
