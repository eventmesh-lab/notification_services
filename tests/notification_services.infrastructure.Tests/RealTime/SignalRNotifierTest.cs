using Microsoft.AspNetCore.SignalR;
using Moq;
using notification_services.infrastructure.Hubs;
using notification_services.infrastructure.RealTime;
using Xunit;

namespace notification_services.infrastructure.Tests.RealTime
{
    public class SignalRNotifierTests
    {
        private readonly Mock<IHubContext<NotificationHub>> _hubContextMock;
        private readonly Mock<IHubClients> _clientsMock;
        private readonly Mock<ISingleClientProxy> _clientProxyMock;
        private readonly SignalRNotifier _notifier;

        public SignalRNotifierTests()
        {
            _hubContextMock = new Mock<IHubContext<NotificationHub>>();
            _clientsMock = new Mock<IHubClients>();
            _clientProxyMock = new Mock<ISingleClientProxy>();

            _hubContextMock.Setup(h => h.Clients).Returns(_clientsMock.Object);

            _notifier = new SignalRNotifier(_hubContextMock.Object);
        }

        [Fact]
        public async Task SendToUserAsync_ShouldSendMessageToSpecificUserGroup()
        {
            var userId = "user123";
            var amount = "100.00";
            var expectedMessage = $"Pago de {amount} exitoso";

            _clientsMock.Setup(c => c.Group(userId)).Returns(_clientProxyMock.Object);

            await _notifier.SendToUserAsync(userId, amount);

            _clientProxyMock.Verify(
                p => p.SendCoreAsync(
                    "PagoCompletado",
                    It.Is<object[]>(o => o.Length == 1 && (string)o[0] == expectedMessage),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task SendToUserConfirmedReservationAsync_ShouldSendMessageToUserGroup()
        {
            var userId = "user456";
            var expectedMessage = "Reserva confirmada";

            _clientsMock.Setup(c => c.Group(userId)).Returns(_clientProxyMock.Object);

            await _notifier.SendToUserConfirmedReservationAsync(userId);

            _clientProxyMock.Verify(
                p => p.SendCoreAsync(
                    "ReservaCompletada",
                    It.Is<object[]>(o => o.Length == 1 && (string)o[0] == expectedMessage),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task SendToUserAsync_ShouldThrowException_WhenHubFails()
        {
            var userId = "user789";
            _clientsMock.Setup(c => c.Group(userId)).Throws(new Exception("SignalR Error"));

            await Assert.ThrowsAsync<Exception>(() =>
                _notifier.SendToUserAsync(userId, "50"));
        }
    }
}