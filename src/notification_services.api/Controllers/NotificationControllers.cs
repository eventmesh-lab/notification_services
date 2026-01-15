using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using notification_services.application.Commands.Commands;
using notification_services.application.DTOs;
using notification_services.application.Interfaces;
using notification_services.infrastructure.Hubs;

namespace notification_services.api.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationControllers : ControllerBase
    {

        private readonly IMediator _mediator;
        //    private readonly IHubContext<notification_services.infrastructure.Hubs.NotificationHub> _hubContext;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationControllers(IMediator mediator, IHubContext<NotificationHub> hubContext)
        {
            _mediator = mediator;
         //   _hubContext = hubContext;
        }

        [HttpPost("paymentSuccessNotification")]
        public async Task<IActionResult> PaymentSuccesAlert([FromBody] PaymentSuccessNotificationDto dto)
        {

            if (string.IsNullOrEmpty(dto.Email))
            {
                Console.WriteLine("ERROR: El email llegó vacío.");
                return BadRequest("El email es obligatorio");
            }
            // await _hubContext.Clients.Group(dto.Email).SendAsync("PagoCompletado", $"Pago recibido: {dto.Amount}");
            await _mediator.Send(new SendPaymentNotificationCommand(dto));
            //await _hubContext.Clients.All.SendAsync("PagoCompletado", "¡Prueba de notificación general!");
            return Ok("Notificación enviada y guardada.");
        }

        [HttpPost("ConfirmedReservationNotification")]
        public async Task<IActionResult> ConfirmedReservationAlert([FromBody] ConfirmedReservationDto dto)
        {

            if (string.IsNullOrEmpty(dto.Email))
            {
                return BadRequest("El email es obligatorio");
            }
            await _mediator.Send(new SendConfirmedReservationCommand(dto));
            return Ok("Notificación enviada y guardada.");
        }

        [HttpPost("paymentSuccessNotificationEmail")]
        public async Task<IActionResult> EnviarCorreoPagoExitoso([FromBody] EnviarCorreoPagoExitosoDto pagoDto)
        {
            var resultado = await _mediator.Send(new SendEmailPaymendNotificationCommand(pagoDto));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El correo ha sido enviado exitosamente .", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El correo no puedo ser enviado exitosamente .", Exito = false });
        }

        [HttpPost("reservaNotificationEmail")]
        public async Task<IActionResult> EnviarCorreoReservaConfirmada([FromBody] EnviarCorreoReservaExitosaDto Dto)
        {
            var resultado = await _mediator.Send(new SendEmailConfirmedReservationCommand(Dto));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El correo ha sido enviado exitosamente .", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El correo no puedo ser enviado exitosamente .", Exito = false });
        }
    }
}
