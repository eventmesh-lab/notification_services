using Microsoft.AspNetCore.SignalR;
using notification_services.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notification_services.infrastructure.Hubs;

namespace notification_services.infrastructure.RealTime
{
    public class SignalRNotifier : IRealTimeNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUserAsync(string userId, string amount)
        {
            Console.WriteLine("Mensaje");
            await _hubContext.Clients.Group(userId).SendAsync("PagoCompletado", $"Pago de {amount} exitoso");
            Console.WriteLine("Mensaje2");
        }

        public async Task SendToUserConfirmedReservationAsync(string userId)
        {
            await _hubContext.Clients.Group(userId).SendAsync("ReservaCompletada", $"Reserva confirmada");
        }
    }
}
