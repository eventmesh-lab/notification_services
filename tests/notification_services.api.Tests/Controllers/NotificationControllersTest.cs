using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using notification_services.api.Controllers;
using notification_services.application.Commands.Commands;
using notification_services.application.DTOs;
using notification_services.infrastructure.Hubs;
using Xunit;

namespace notification_services.tests.Api
{
    public class NotificationControllersTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IHubContext<NotificationHub>> _hubContextMock;
        private readonly NotificationControllers _controller;

        public NotificationControllersTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _hubContextMock = new Mock<IHubContext<NotificationHub>>();
            _controller = new NotificationControllers(_mediatorMock.Object, _hubContextMock.Object);
        }

        [Fact]
        public async Task PaymentSuccesAlert_ShouldReturnOk_WhenDtoIsValid()
        {
            var dto = new PaymentSuccessNotificationDto ("test@test.com", "100" );
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendPaymentNotificationCommand>(), default))
                .ReturnsAsync(true);

            var result = await _controller.PaymentSuccesAlert(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notificación enviada y guardada.", okResult.Value);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SendPaymentNotificationCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task PaymentSuccesAlert_ShouldReturnBadRequest_WhenEmailIsEmpty()
        {
            var dto = new PaymentSuccessNotificationDto("", "100");

            var result = await _controller.PaymentSuccesAlert(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("El email es obligatorio", badRequestResult.Value);
        }

        [Fact]
        public async Task ConfirmedReservationAlert_ShouldReturnOk_WhenDtoIsValid()
        {
            var dto = new ConfirmedReservationDto ( "test@test.com" );
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendConfirmedReservationCommand>(), default))
                .ReturnsAsync(true);

            var result = await _controller.ConfirmedReservationAlert(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notificación enviada y guardada.", okResult.Value);
        }

        [Fact]
        public async Task EnviarCorreoPagoExitoso_ShouldReturnOk_WhenMediatorReturnsTrue()
        {
            var dto = new EnviarCorreoPagoExitosoDto { Destinatario = "test@test.com" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendEmailPaymendNotificationCommand>(), default))
                .ReturnsAsync(true);

            var result = await _controller.EnviarCorreoPagoExitoso(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResultadoDTO>(okResult.Value);
            Assert.True(responseDto.Exito);
            Assert.Contains("exitosamente", responseDto.Mensaje);
        }

        [Fact]
        public async Task EnviarCorreoPagoExitoso_ShouldReturnBadRequest_WhenMediatorReturnsFalse()
        {
            var dto = new EnviarCorreoPagoExitosoDto();
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendEmailPaymendNotificationCommand>(), default))
                .ReturnsAsync(false);

            var result = await _controller.EnviarCorreoPagoExitoso(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseDto = Assert.IsType<ResultadoDTO>(badRequestResult.Value);
            Assert.False(responseDto.Exito);
        }

        [Fact]
        public async Task EnviarCorreoReservaConfirmada_ShouldReturnOk_WhenMediatorReturnsTrue()
        {
            var dto = new EnviarCorreoReservaExitosaDto { Destinatario = "test@test.com" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendEmailConfirmedReservationCommand>(), default))
                .ReturnsAsync(true);

            var result = await _controller.EnviarCorreoReservaConfirmada(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResultadoDTO>(okResult.Value);
            Assert.True(responseDto.Exito);
        }

        [Fact]
        public async Task EnviarCorreoReservaConfirmada_ShouldReturnBadRequest_WhenMediatorReturnsFalse()
        {
            var dto = new EnviarCorreoReservaExitosaDto();
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendEmailConfirmedReservationCommand>(), default))
                .ReturnsAsync(false);

            var result = await _controller.EnviarCorreoReservaConfirmada(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseDto = Assert.IsType<ResultadoDTO>(badRequestResult.Value);
            Assert.False(responseDto.Exito);
        }
    }
}