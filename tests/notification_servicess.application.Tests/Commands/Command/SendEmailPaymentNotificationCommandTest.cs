using Xunit;
using notification_services.application.Commands.Commands;
using notification_services.application.DTOs;

namespace notification_services.tests.Application.Commands
{
    public class SendEmailPaymendNotificationCommandTests
    {
        [Fact]
        public void Constructor_ShouldInitializePagoDtoProperty()
        {
            var dto = new EnviarCorreoPagoExitosoDto
            {
                Destinatario = "cliente@test.com",
                MontoPago = "250.00 USD",
                FechaPago = DateTime.UtcNow
            };

            var command = new SendEmailPaymendNotificationCommand(dto);

            Assert.NotNull(command.pagoDto);
            Assert.Equal(dto, command.pagoDto);
            Assert.Equal("cliente@test.com", command.pagoDto.Destinatario);
        }

        [Fact]
        public void Property_ShouldBeSettable()
        {
            var initialDto = new EnviarCorreoPagoExitosoDto { Destinatario = "viejo@test.com" };
            var command = new SendEmailPaymendNotificationCommand(initialDto);
            var newDto = new EnviarCorreoPagoExitosoDto { Destinatario = "nuevo@test.com" };

            command.pagoDto = newDto;

            Assert.Equal(newDto, command.pagoDto);
            Assert.Equal("nuevo@test.com", command.pagoDto.Destinatario);
        }

        [Fact]
        public void Command_ShouldImplementIRequestWithBooleanReturnType()
        {
            var dto = new EnviarCorreoPagoExitosoDto();
            var command = new SendEmailPaymendNotificationCommand(dto);

            Assert.IsAssignableFrom<MediatR.IRequest<bool>>(command);
        }
    }
}