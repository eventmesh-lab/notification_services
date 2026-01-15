using MassTransit;
using Moq;
using notification_services.application.Commands.Commands;
using notification_services.application.Commands.Handlers;
using notification_services.application.DTOs;
using notification_services.application.Interfaces;
using notification_services.domain.Entities;
using Xunit;
using Events.Shared;

namespace notification_services.tests.Handlers
{
    public class SendEmailPaymendNotificationHandlerTests
    {
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly SendEmailPaymendNotificationHandler _handler;

        public SendEmailPaymendNotificationHandlerTests()
        {
            _emailSenderMock = new Mock<IEmailSender>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _handler = new SendEmailPaymendNotificationHandler(_emailSenderMock.Object, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenEmailAndPublishSucceed()
        {
            var dto = new EnviarCorreoPagoExitosoDto
            {
                Destinatario = "cliente@test.com",
                MontoPago = "150 USD",
                FechaPago = DateTime.UtcNow
            };
            var command = new SendEmailPaymendNotificationCommand(dto);

            _emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            _publishEndpointMock.Setup(x => x.Publish<IAuditLogCreated>(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            _emailSenderMock.Verify(x => x.SendEmailAsync(It.IsAny<Notification>()), Times.Once);
            _publishEndpointMock.Verify(x => x.Publish<IAuditLogCreated>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenEmailSenderFails()
        {
            var dto = new EnviarCorreoPagoExitosoDto { Destinatario = "error@test.com" };
            var command = new SendEmailPaymendNotificationCommand(dto);

            _emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<Notification>()))
                .ThrowsAsync(new Exception("Fail"));

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Ha ocurrido un error al enviar el correo al usuario", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenPublishFails()
        {
            var dto = new EnviarCorreoPagoExitosoDto { Destinatario = "cliente@test.com" };
            var command = new SendEmailPaymendNotificationCommand(dto);

            _emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            _publishEndpointMock.Setup(x => x.Publish<IAuditLogCreated>(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Bus error"));

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}