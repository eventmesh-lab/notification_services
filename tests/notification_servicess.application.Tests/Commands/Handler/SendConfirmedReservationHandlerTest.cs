using Moq;
using notification_services.application.Commands.Commands;
using notification_services.application.Commands.Handlers;
using notification_services.application.DTOs;
using notification_services.application.Interfaces;
using notification_services.domain.Interfaces;
using Xunit;

namespace notification_services.tests.Handlers
{
    public class SendConfirmedReservationHandlerTests
    {
        private readonly Mock<INotificationRepositoryPostgres> _repositoryMock;
        private readonly Mock<IRealTimeNotifier> _notifierMock;
        private readonly SendConfirmedReservationHandler _handler;

        public SendConfirmedReservationHandlerTests()
        {
            _repositoryMock = new Mock<INotificationRepositoryPostgres>();
            _notifierMock = new Mock<IRealTimeNotifier>();
            _handler = new SendConfirmedReservationHandler(_repositoryMock.Object, _notifierMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallNotifierAndReturnTrue_WhenRequestIsValid()
        {
            var email = "test@example.com";
            var dto = new ConfirmedReservationDto (email);
            var command = new SendConfirmedReservationCommand(dto);

            _notifierMock.Setup(n => n.SendToUserConfirmedReservationAsync(email))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            _notifierMock.Verify(n => n.SendToUserConfirmedReservationAsync(email), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldRethrowException_WhenNotifierFails()
        {
            var dto = new ConfirmedReservationDto ("error@test.com");
            var command = new SendConfirmedReservationCommand(dto);

            _notifierMock.Setup(n => n.SendToUserConfirmedReservationAsync(It.IsAny<string>()))
                .ThrowsAsync(new System.Exception("Error de conexión SignalR"));

            await Assert.ThrowsAsync<System.Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}