using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notification_services.domain.Entities;

namespace notification_services.application.Interfaces
{
    public interface IEmailSender
    {

        Task SendEmailAsync(Notification notificacion);
    }
}
