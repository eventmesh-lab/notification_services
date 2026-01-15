using Xunit;
using notification_services.application.Commands.Commands;
using notification_services.application.DTOs;

namespace notification_services.tests.Application.Commands
{
    public class SendEmailConfirmedReservationCommandTests
    {
        [Fact]
        public void Constructor_ShouldInitializeDtoProperty()
        {
            var dto = new EnviarCorreoReservaExitosaDto
            {
                Destinatario = "usuario@test.com",
                NombreEvento = "Concierto de Rock",
                IdReserva = "123",
                CantidadTickets = "3",
                FechaCreacion = DateTime.UtcNow
            };

            var command = new SendEmailConfirmedReservationCommand(dto);

            Assert.NotNull(command.Dto);
            Assert.Equal(dto, command.Dto);
            Assert.Equal("usuario@test.com", command.Dto.Destinatario);
        }

        [Fact]
        public void Property_ShouldBeSettable()
        {
            var initialDto = new EnviarCorreoReservaExitosaDto { Destinatario = "inicial@test.com" };
            var command = new SendEmailConfirmedReservationCommand(initialDto);
            var newDto = new EnviarCorreoReservaExitosaDto { Destinatario = "nuevo@test.com" };

            command.Dto = newDto;

            Assert.Equal(newDto, command.Dto);
            Assert.Equal("nuevo@test.com", command.Dto.Destinatario);
        }

        [Fact]
        public void Command_ShouldImplementIRequestWithBooleanReturnType()
        {
            var dto = new EnviarCorreoReservaExitosaDto();
            var command = new SendEmailConfirmedReservationCommand(dto);

            Assert.IsAssignableFrom<MediatR.IRequest<bool>>(command);
        }
    }
}