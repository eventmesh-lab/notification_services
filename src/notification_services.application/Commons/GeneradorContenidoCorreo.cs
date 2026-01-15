using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notification_services.application.DTOs;

namespace notification_services.application.Commons
{
    public static class GeneradorContenidoCorreo
    {
        public static ContenidoCorreoDTO PagoExitoso(string monto, DateTime fechaPago)
        {
            var cuerpo = $@"
                        <p>¡Pago realizado exitosamente! 🎉</p>
                        <p>Has completado tu transacción de manera correcta.</p>
                        <p>Detalles de la operación:</p>
                        <ul>
                             <li><strong>Monto pagado:</strong> ${monto:F2}</li>
                             <li><strong>Fecha de pago:</strong> {fechaPago:dd/MM/yyyy}</li>
                        </ul>
                        <p>A continuación, los pasos para acceder a tu compra:</p>
                        <ol>
                             <li>Ingresa al módulo correspondiente en la plataforma.</li>
                             <li>Selecciona la transacción realizada.</li>
                             <li>Visualiza el comprobante o acceso electrónico.</li>
                             <li>Utiliza el comprobante según las instrucciones indicadas.</li>
                        </ol>
                        <p>Gracias por tu pago. ¡Esperamos que disfrutes la experiencia!</p>";
  

            return new ContenidoCorreoDTO
            {
                Asunto = " Confirmación de pago",
                Html = cuerpo

            };
        }

        public static ContenidoCorreoDTO ReservaExitosa(string nombreEvento, string idReserva, string cantidadTickets, DateTime fechaReserva)
        {
            var cuerpo = $@"
                <p>¡Reserva realizada exitosamente! 🎉</p>
                <p>Has asegurado tu lugar para el evento <strong>{nombreEvento}</strong>.</p>
                <p>Detalles de la reserva:</p>
                <ul>
                    <li><strong>Número de reserva:</strong> {idReserva}</li>
                    <li><strong>Cantidad de tickets:</strong> {cantidadTickets}</li>
                    <li><strong>Fecha de reserva:</strong> {fechaReserva:dd/MM/yyyy}</li>
                </ul>
                <p>Próximos pasos para completar tu compra:</p>
                <ol>
                    <li>Ingresa al módulo de <strong>Mis Reservas</strong> en la plataforma.</li>
                    <li>Selecciona la reserva correspondiente al evento <strong>{nombreEvento}</strong>.</li>
                    <li>Realiza el pago dentro del plazo indicado para confirmar tus tickets.</li>
                    <li>Una vez confirmado el pago, podrás visualizar tus tickets electrónicos.</li>
                </ol>
                <p>Gracias por tu reserva. ¡Estamos emocionados de verte en el evento!</p>";

            return new ContenidoCorreoDTO
            {
                Asunto = " Confirmación de reserva",
                Html = cuerpo
            };
        }
    }
}
