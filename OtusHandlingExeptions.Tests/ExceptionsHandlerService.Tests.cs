using System;

using FluentAssertions;

using Moq;

using OtusHandlingExeptions.Commands;
using OtusHandlingExeptions.Interfaces;
using OtusHandlingExeptions.Services;

using Xunit;

namespace OtusHandlingExeptions.Tests
{
    public class ExceptionsHandlerServiceTests
    {
        private readonly Mock<ILogger> _logger = new Mock<ILogger>();
        private readonly ExceptionsHandlerService _exceptionsHandlerService;
        public ExceptionsHandlerServiceTests()
        {
            _exceptionsHandlerService = new ExceptionsHandlerService(_logger.Object);
        }

        [Fact]
        public void RegisterHandler_ShouldThrowException_WhenRegisteringNullDelegate()
        {
            var result = () => _exceptionsHandlerService.RegisterHandler(null as Action<ICommand, Exception>);

            result.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RegisterHandler_ShouldRegisterHandler()
        {
            bool wasHandlerInvoked = false;
            Action<ExceptionCommand, NotImplementedException> handler = (c, e) => wasHandlerInvoked = true;
            _exceptionsHandlerService.RegisterHandler(handler);
            ICommand cmd = new ExceptionCommand();
            Exception exc = new NotImplementedException();

            _exceptionsHandlerService.Handle(cmd, exc);

            wasHandlerInvoked.Should().BeTrue();
        }

        [Fact]
        public void RegisterHandler_ShouldReregister_WhenProvidedKeyAlreadyExists()
        {
            bool handler1Invoked = false;
            bool handler2Invoked = false;
            Action<ExceptionCommand, NotImplementedException> handler1 = (c, e) => handler1Invoked = true;
            Action<ExceptionCommand, NotImplementedException> handler2 = (c, e) => handler2Invoked = true;
            _exceptionsHandlerService.RegisterHandler(handler1);
            _exceptionsHandlerService.RegisterHandler(handler2);
            ICommand cmd = new ExceptionCommand();
            Exception exc = new NotImplementedException();

            _exceptionsHandlerService.Handle(cmd, exc);

            handler1Invoked.Should().BeFalse();
            handler2Invoked.Should().BeTrue();
        }

        [Fact]
        public void GetHandler_ShouldLogAndNotHandle_WhenProvidedKeyDoesNotExist()
        {
            bool wasHandlerInvoked = false;
            Action<ExceptionCommand, NotImplementedException> handler = (c, e) => wasHandlerInvoked = true;
            ICommand cmd = new ExceptionCommand();
            Exception exc = new NotImplementedException();

            _exceptionsHandlerService.Handle(cmd, exc);

            _logger.Verify(l => l.Log(It.IsAny<string>()), Times.Once());
            wasHandlerInvoked.Should().BeFalse();
        }
    }
}
