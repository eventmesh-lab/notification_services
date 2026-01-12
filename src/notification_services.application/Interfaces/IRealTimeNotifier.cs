using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.application.Interfaces
{
    public interface IRealTimeNotifier
    {
        Task SendToUserAsync(string userId, string amount);
        Task SendToUserConfirmedReservationAsync(string userId);
    }
}
