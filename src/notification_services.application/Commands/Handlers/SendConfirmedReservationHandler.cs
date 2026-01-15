using MediatR;
using notification_services.application.Commands.Commands;
using notification_services.application.Interfaces;
using notification_services.domain.Entities;
using notification_services.domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.application.Commands.Handlers
{
    public class SendConfirmedReservationHandler : IRequestHandler<SendConfirmedReservationCommand, bool>
    {
        private readonly INotificationRepositoryPostgres _repository;
        private readonly IRealTimeNotifier _notifier;

        public SendConfirmedReservationHandler(INotificationRepositoryPostgres repository, IRealTimeNotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }

        public async Task<bool> Handle(SendConfirmedReservationCommand request, CancellationToken cancellationToken)
        {
          //  string mensaje = $"Tu confirmacion de reserva fue exitosa.";
         //   var notification = new Notification(request.Dto.Email, mensaje);

            //    await _repository.AddAsync(notification);
            await _notifier.SendToUserConfirmedReservationAsync(request.Dto.Email);
            return true;
        }
    }
}
