
using System;

using FluentAssertions;

using Moq;

using OtusHandlingExeptions.Commands;
using OtusHandlingExeptions.Interfaces;
using OtusHandlingExeptions.Services;

using Xunit;

namespace OtusHandlingExeptions.Tests
{
    public class CommandTaskRunnerTests
    {
        private readonly CommandTaskRunner _runner;
        private readonly Mock<ILogger> _logger = new Mock<ILogger>();
        private readonly CommandQueue _queue = new CommandQueue();
        private readonly ExceptionsHandlerService _handlerService;

        public CommandTaskRunnerTests()
        {
            _handlerService = new ExceptionsHandlerService(_logger.Object);
            _runner = new CommandTaskRunner(_logger.Object, _queue, _handlerService);
        }

        [Fact]
        public void Exception_Plus_Log_Strategy()
        {
            var commandLogger = new Mock<ILogger>();
            var exCommand = new ExceptionCommand();
            _queue.Enqueue(exCommand);
            void HandleExceptionCommand(ExceptionCommand command, NotImplementedException exception)
            {
                _queue.Enqueue(new LoggerCommand(commandLogger.Object, "logged error"));
            }
            _handlerService.RegisterHandler<ExceptionCommand, NotImplementedException>(HandleExceptionCommand);

            _runner.Run();

            commandLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Exception_Plus_Repeat_Plus_Log_Strategy()
        {
            var commandLogger = new Mock<ILogger>();
            var exCommand = new ExceptionCommand();
            RepeaterCommand repeater = null;
            _queue.Enqueue(exCommand);
            void HandleExceptionCommand(ExceptionCommand command, NotImplementedException exception)
            {
                repeater = new RepeaterCommand(command);
                _queue.Enqueue(repeater);
            }
            void HandleRepeaterCommand(RepeaterCommand command, NotImplementedException exception)
            {
                _queue.Enqueue(new LoggerCommand(commandLogger.Object, "logged error"));
            }
            _handlerService.RegisterHandler<ExceptionCommand, NotImplementedException>(HandleExceptionCommand);
            _handlerService.RegisterHandler<RepeaterCommand, NotImplementedException>(HandleRepeaterCommand);

            _runner.Run();

            repeater.Should().NotBeNull();
            commandLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Exception_Plus_RepeatException_Plus_Repeat_Plus_Log_Strategy()
        {
            var commandLogger = new Mock<ILogger>();
            var exCommand = new ExceptionCommand();
            DoubleRepeaterCommand doubleRepeater = null;
            RepeaterCommand repeater = null;
            _queue.Enqueue(exCommand);
            void HandleExceptionCommand(ExceptionCommand command, NotImplementedException exception)
            {
                doubleRepeater = new DoubleRepeaterCommand(command);
                _queue.Enqueue(doubleRepeater);
            }
            void HandleRepeatExceptionCommand(DoubleRepeaterCommand command, NotImplementedException exception)
            {
                repeater = new RepeaterCommand(command);
                _queue.Enqueue(repeater);
            }
            void HandleRepeaterCommand(RepeaterCommand command, NotImplementedException exception)
            {
                _queue.Enqueue(new LoggerCommand(commandLogger.Object, "logged error"));
            }
            _handlerService.RegisterHandler<ExceptionCommand, NotImplementedException>(HandleExceptionCommand);
            _handlerService.RegisterHandler<DoubleRepeaterCommand, NotImplementedException>(HandleRepeatExceptionCommand);
            _handlerService.RegisterHandler<RepeaterCommand, NotImplementedException>(HandleRepeaterCommand);

            _runner.Run();

            doubleRepeater.Should().NotBeNull();
            repeater.Should().NotBeNull();
            commandLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once);
        }
    }
}
