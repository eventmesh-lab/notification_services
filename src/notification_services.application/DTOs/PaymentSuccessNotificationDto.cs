using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.application.DTOs
{
    public record PaymentSuccessNotificationDto(string Email, string Amount);

}
