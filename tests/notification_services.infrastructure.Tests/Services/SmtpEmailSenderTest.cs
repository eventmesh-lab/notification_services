using Microsoft.Extensions.Configuration;
using Moq;
using notification_services.domain.Entities;
using notification_services.domain.ValueObjects;
using notification_services.infrastructure.Services;
using Xunit;

namespace notification_services.tests.Infrastructure.Services
{
    public class SmtpEmailSenderTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly SmtpEmailSender _service;

        public SmtpEmailSenderTests()
        {
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["Smtp:Host"]).Returns("smtp.test.com");
            _configurationMock.Setup(c => c["Smtp:Port"]).Returns("587");
            _configurationMock.Setup(c => c["Smtp:User"]).Returns("user@test.com");
            _configurationMock.Setup(c => c["Smtp:Password"]).Returns("password123");
            _configurationMock.Setup(c => c["Smtp:From"]).Returns("noreply@test.com");

            _service = new SmtpEmailSender(_configurationMock.Object);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldReadConfigurationsCorrectly()
        {
            var destinatario = new DestinatarioNotificacionVO("cliente@gmail.com");
            var contenido = new ContenidoNotificacionVO("<h1>Pago Exitoso</h1>");
            var asunto = new AsuntoNotificacionVO("Confirmación de Pago");

            var notification = new Notification(destinatario, contenido, asunto);


            var exception = await Record.ExceptionAsync(() => _service.SendEmailAsync(notification));


            _configurationMock.Verify(c => c["Smtp:Host"], Times.AtLeastOnce);
            _configurationMock.Verify(c => c["Smtp:Port"], Times.AtLeastOnce);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowException_WhenPortIsInvalid()
        {
            _configurationMock.Setup(c => c["Smtp:Port"]).Returns("NoSoyUnNumero");
            var notification = new Notification(
                new DestinatarioNotificacionVO("test@test.com"),
                new ContenidoNotificacionVO("Body"),
                new AsuntoNotificacionVO("Subject")
            );

            await Assert.ThrowsAsync<FormatException>(() => _service.SendEmailAsync(notification));
        }
    }
}