using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.Common.Interfaces
{
    public interface INotificationService
    {
        public Task SendToAllAsync(string message);
        public Task SendToUserAsync(string userId, string message);
        public Task SendToGroupAsync(string group, string message);
    }
}
