using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CampusNova.Api.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(
       Context.User?.FindFirst("authId")?.Value);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine(
                $"Usuario desconectado: {Context.UserIdentifier}");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
