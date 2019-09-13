using System.Threading.Tasks;
using FbRestaurantsBot.Api.Controllers;
using FbRestaurantsBot.Core.Exceptions;
using FbRestaurantsBot.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FbRestaurantsBot.Tests
{
    public class WebHookControllerTests
    {
        [Fact]
        public void Verify_ByDefault_ReturnsOkObjectResult()
        {
            var messengerServiceStub = new Mock<IMessengerService>();

            var controller = new WebHookController(messengerServiceStub.Object);
            var result = controller.Verify
                (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Verify_ByDefault_CallsMessengerService()
        {
            var messengerServiceMock = new Mock<IMessengerService>();

            var controller = new WebHookController(messengerServiceMock.Object);
            var result = controller.Verify
                (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            messengerServiceMock.Verify(e => e.VerifyToken
                (It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Verify_WhenVerificationFailed_ThrowsVerificationException()
        {
            var messengerServiceStub = new Mock<IMessengerService>();
            messengerServiceStub.Setup(e => e.VerifyToken(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<VerificationException>();
            
            var controller = new WebHookController(messengerServiceStub.Object);

            Assert.Throws<VerificationException>(() =>
                controller.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task Receive_ByDefault_ReturnsOkResult()
        {
            var messengerServiceStub = new Mock<IMessengerService>();
            
            var controller = new WebHookController(messengerServiceStub.Object);
            var result = await controller.Receive();

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Receive_ByDefault_CallsMessengerService()
        {
            var messengerServiceMock = new Mock<IMessengerService>();
            
            var controller = new WebHookController(messengerServiceMock.Object);
            var result = await controller.Receive();

            messengerServiceMock.Verify(e => e.Receive(It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public async Task Receive_WhenIncorrectRequestBody_ThrowsMessengerException()
        {
            var messengerServiceMock = new Mock<IMessengerService>();
            messengerServiceMock.Setup(e => e.Receive(It.IsAny<HttpRequest>()))
                .Throws<MessengerException>();
            
            var controller = new WebHookController(messengerServiceMock.Object);

            await Assert.ThrowsAsync<MessengerException>
                (() => controller.Receive());
        }
    }
}