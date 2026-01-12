using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace notification_services.infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task RegistrarUsuario(string userId)
        {
            try
            {

                if (string.IsNullOrEmpty(userId))
                {
                    return;
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}