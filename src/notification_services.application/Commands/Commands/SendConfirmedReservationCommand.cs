using MediatR;
using notification_services.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.application.Commands.Commands
{
    public class SendConfirmedReservationCommand : IRequest<bool>
    {
        public ConfirmedReservationDto Dto { get; set; }

        public SendConfirmedReservationCommand(ConfirmedReservationDto dto)
        {
            Dto = dto; ;
        }
    }
}
