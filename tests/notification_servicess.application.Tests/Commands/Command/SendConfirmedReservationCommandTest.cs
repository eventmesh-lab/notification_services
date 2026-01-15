using Xunit;
using notification_services.application.Commands.Commands;
using notification_services.application.DTOs;

namespace notification_services.tests.Application.Commands
{
    public class SendConfirmedReservationCommandTests
    {
        [Fact]
        public void Constructor_ShouldInitializeDtoProperty()
        {
            var dto = new ConfirmedReservationDto
            (
                 "test@example.com"
            );

            var command = new SendConfirmedReservationCommand(dto);

            Assert.NotNull(command.Dto);
            Assert.Equal(dto, command.Dto);
            Assert.Equal("test@example.com", command.Dto.Email);
        }


        [Fact]
        public void Command_ShouldImplementIRequestWithBooleanReturnType()
        {
            var dto = new ConfirmedReservationDto("test@gmail.com");
            var command = new SendConfirmedReservationCommand(dto);

            Assert.IsAssignableFrom<MediatR.IRequest<bool>>(command);
        }
    }
}