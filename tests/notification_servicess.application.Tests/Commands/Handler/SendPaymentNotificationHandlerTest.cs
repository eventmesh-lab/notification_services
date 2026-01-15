using Moq;
using notification_services.application.Commands.Commands;
using notification_services.application.Commands.Handlers;
using notification_services.application.DTOs;
using notification_services.application.Interfaces;
using notification_services.domain.Interfaces;
using Xunit;

namespace notification_services.tests.Handlers
{
    public class SendPaymentNotificationHandlerTests
    {
        private readonly Mock<INotificationRepositoryPostgres> _repositoryMock;
        private readonly Mock<IRealTimeNotifier> _notifierMock;
        private readonly SendPaymentNotificationHandler _handler;

        public SendPaymentNotificationHandlerTests()
        {
            _repositoryMock = new Mock<INotificationRepositoryPostgres>();
            _notifierMock = new Mock<IRealTimeNotifier>();
            _handler = new SendPaymentNotificationHandler(_repositoryMock.Object, _notifierMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallRealTimeNotifierAndReturnTrue()
        {
            var dto = new PaymentSuccessNotificationDto
            (
                 "test@example.com",
                 "150.00"
            );
            var command = new SendPaymentNotificationCommand(dto);

            _notifierMock.Setup(n => n.SendToUserAsync(dto.Email, dto.Amount))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            _notifierMock.Verify(n => n.SendToUserAsync(dto.Email, dto.Amount), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldRethrowException_WhenNotifierFails()
        {
            var dto = new PaymentSuccessNotificationDto
            (
                "error@example.com",
                "150.00"
            ); var command = new SendPaymentNotificationCommand(dto);

            _notifierMock.Setup(n => n.SendToUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new System.Exception("SignalR connection error"));

            await Assert.ThrowsAsync<System.Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}