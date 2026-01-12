using MediatR;
using notification_services.application.Commons;
using notification_services.domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notification_services.application.Commands.Commands;
using notification_services.application.Interfaces;
using notification_services.domain.Entities;

namespace notification_services.application.Commands.Handlers
{
    public class SendEmailPaymendNotificationHandler : IRequestHandler<SendEmailPaymendNotificationCommand, bool>
    {
        private readonly IEmailSender _emailSender;

        public SendEmailPaymendNotificationHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<bool> Handle(SendEmailPaymendNotificationCommand request, CancellationToken cancellationToken)
        {

            try
            {
                // Se genera el contenido del correo a enviar.
                var contenidoCorreo = GeneradorContenidoCorreo.PagoExitoso(request.pagoDto.NombreEvento, request.pagoDto.IdReserva, request.pagoDto.MontoPago, request.pagoDto.FechaPago);

                //Se crea la instancia de la entidad Notificacion
                var notificacion = new Notification(new DestinatarioNotificacionVO(request.pagoDto.Destinatario), new ContenidoNotificacionVO(contenidoCorreo.Html), new AsuntoNotificacionVO(contenidoCorreo.Asunto));

                //Se envia el correo personalizado al usuario que pago de la reserva
                await _emailSender.SendEmailAsync(notificacion);

                return true;

            }
            catch (System.Exception ex)
            {
                throw new ArgumentException("Ha ocurrido un error al enviar el correo al usuario", ex);
            }
        }
    }
}
