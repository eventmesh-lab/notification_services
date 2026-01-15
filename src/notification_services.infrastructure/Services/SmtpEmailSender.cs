using MediatR;
using Microsoft.Extensions.Configuration;
using notification_services.application.Interfaces;
using notification_services.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.infrastructure.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(Notification notificacion)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:User"], _configuration["Smtp:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:From"]),
                Subject = notificacion.asuntoNotificacion.asuntoNotificacion,
                Body = notificacion.contenidoNotificacion.contenidoNotificacion,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(notificacion.destinatarioNotificacion.destinatarioNotificacion);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
