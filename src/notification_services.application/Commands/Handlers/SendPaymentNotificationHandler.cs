using MediatR;
using notification_services.application.Commands.Commands;
using notification_services.application.Interfaces;
using notification_services.domain.Entities;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notification_services.domain.Interfaces;

namespace notification_services.application.Commands.Handlers
{
    public class SendPaymentNotificationHandler : IRequestHandler<SendPaymentNotificationCommand, bool>
    {
        private readonly INotificationRepositoryPostgres _repository;
        private readonly IRealTimeNotifier _notifier;

        public SendPaymentNotificationHandler(INotificationRepositoryPostgres repository, IRealTimeNotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }

        public async Task<bool> Handle(SendPaymentNotificationCommand request, CancellationToken cancellationToken)
        {
           // string mensaje =  $"Tu pago de ${request.Dto.Amount} fue exitoso.";
         //   var notification = new Notification(request.Dto.Email, mensaje);

            //    await _repository.AddAsync(notification);
            await _notifier.SendToUserAsync(request.Dto.Email, request.Dto.Amount );
            return true;
        }
    }
}
