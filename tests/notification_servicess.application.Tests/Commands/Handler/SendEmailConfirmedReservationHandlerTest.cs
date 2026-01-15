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
    public class SendEmailConfirmedReservationHandlerTests
    {
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly SendEmailConfirmedReservationHandler _handler;

        public SendEmailConfirmedReservationHandlerTests()
        {
            _emailSenderMock = new Mock<IEmailSender>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _handler = new SendEmailConfirmedReservationHandler(_emailSenderMock.Object, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenEmailAndPublishSucceed()
        {
            var dto = new EnviarCorreoReservaExitosaDto
            {
                Destinatario = "usuario@test.com",
                NombreEvento = "Concierto",
                IdReserva = "123",
                CantidadTickets = "2",
                FechaCreacion = DateTime.UtcNow
            };
            var command = new SendEmailConfirmedReservationCommand(dto);

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
            var dto = new EnviarCorreoReservaExitosaDto { Destinatario = "error@test.com" };
            var command = new SendEmailConfirmedReservationCommand(dto);

            _emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<Notification>()))
                .ThrowsAsync(new Exception("SMTP Error"));

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Ha ocurrido un error al enviar el correo", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenPublishEndpointFails()
        {
            var dto = new EnviarCorreoReservaExitosaDto { Destinatario = "usuario@test.com" };
            var command = new SendEmailConfirmedReservationCommand(dto);

            _emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            _publishEndpointMock.Setup(x => x.Publish<IAuditLogCreated>(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("RabbitMQ Down"));

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}