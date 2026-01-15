using Xunit;
using notification_services.application.Commands.Commands;
using notification_services.application.DTOs;

namespace notification_services.tests.Application.Commands
{
    public class SendPaymentNotificationCommandTests
    {
        [Fact]
        public void Constructor_ShouldInitializeDtoProperty()
        {
            var dto = new PaymentSuccessNotificationDto
            (
               "usuario@test.com",
               "150.00"
            );

            var command = new SendPaymentNotificationCommand(dto);

            Assert.NotNull(command.Dto);
            Assert.Equal(dto, command.Dto);
            Assert.Equal("usuario@test.com", command.Dto.Email);
            Assert.Equal("150.00", command.Dto.Amount);
        }

        [Fact]
        public void Property_ShouldBeSettable()
        {
            var initialDto = new PaymentSuccessNotificationDto ("viejo@test.com", "0");
            var command = new SendPaymentNotificationCommand(initialDto);
            var newDto = new PaymentSuccessNotificationDto ("nuevo@test.com", "100");

            command.Dto = newDto;

            Assert.Equal(newDto, command.Dto);
            Assert.Equal("nuevo@test.com", command.Dto.Email);
        }

        [Fact]
        public void Command_ShouldImplementIRequestWithBooleanReturnType()
        {
            var dto = new PaymentSuccessNotificationDto("nuevo@test.com", "100");

            var command = new SendPaymentNotificationCommand(dto);

            Assert.IsAssignableFrom<MediatR.IRequest<bool>>(command);
        }
    }
}