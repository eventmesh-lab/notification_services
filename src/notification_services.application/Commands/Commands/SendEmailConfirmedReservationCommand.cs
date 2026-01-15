using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using notification_services.application.DTOs;

namespace notification_services.application.Commands.Commands
{
    public class SendEmailConfirmedReservationCommand : IRequest<bool>
    {
    public EnviarCorreoReservaExitosaDto Dto { get; set; }

    public SendEmailConfirmedReservationCommand(EnviarCorreoReservaExitosaDto Dto)
    {
        this.Dto = Dto;
    }
    }
}
