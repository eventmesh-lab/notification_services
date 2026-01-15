using Events.Shared;
using MassTransit;
using MediatR;
using notification_services.application.Commands.Commands;
using notification_services.application.Commands.Commands;
using notification_services.application.Commons;
using notification_services.application.Interfaces;
using notification_services.domain.Entities;
using notification_services.domain.Entities;
using notification_services.domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace notification_services.application.Commands.Handlers
{
    public class SendEmailConfirmedReservationHandler : IRequestHandler<SendEmailConfirmedReservationCommand, bool>
    {
        private readonly IEmailSender _emailSender;

        private readonly IPublishEndpoint _publishEndpoint;

        public SendEmailConfirmedReservationHandler(IEmailSender emailSender, IPublishEndpoint publishEndpoint)
        {
            _emailSender = emailSender;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(SendEmailConfirmedReservationCommand request, CancellationToken cancellationToken)
        {

            try
            {
                // Se genera el contenido del correo a enviar.
                var contenidoCorreo = GeneradorContenidoCorreo.ReservaExitosa(request.Dto.NombreEvento,request.Dto.IdReserva,
                                                                                request.Dto.CantidadTickets, request.Dto.FechaCreacion);

                //Se crea la instancia de la entidad Notificacion
                var notificacion = new Notification(new DestinatarioNotificacionVO(request.Dto.Destinatario),
                    new ContenidoNotificacionVO(contenidoCorreo.Html),
                    new AsuntoNotificacionVO(contenidoCorreo.Asunto));

                //Se envia el correo personalizado al usuario que reservo tickets
                await _emailSender.SendEmailAsync(notificacion);
                await _publishEndpoint.Publish<IAuditLogCreated>(new
                {
                    Id = Guid.NewGuid(),
                    ServicioOrigen = "notification_services",
                    UsuarioId = request.Dto.Destinatario,
                    TipoAccion = "Registro de pago",
                    Datos = JsonSerializer.Serialize(new { Accion = "notificacion de pago realizado via SignalR" }),
                    FechaOcurrencia = DateTime.UtcNow,
                    Nivel = "Info"
                }, cancellationToken);
                return true;

            }
            catch (System.Exception ex)
            {
                throw new ArgumentException("Ha ocurrido un error al enviar el correo al usuario", ex);
            }
        }
    }
}