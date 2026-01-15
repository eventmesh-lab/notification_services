using MediatR;
using notification_services.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.application.Commands.Commands
{
    public class SendEmailPaymendNotificationCommand : IRequest<bool>
    {
        public EnviarCorreoPagoExitosoDto pagoDto { get; set; }

        public SendEmailPaymendNotificationCommand(EnviarCorreoPagoExitosoDto pagoDto)
        {
            this.pagoDto = pagoDto;
        }
    }
}
