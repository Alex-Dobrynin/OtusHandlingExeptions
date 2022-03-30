using System;

using FluentAssertions;

using Moq;

using OtusHandlingExeptions.Commands;
using OtusHandlingExeptions.Interfaces;

using Xunit;

namespace OtusHandlingExeptions.Tests
{
    public class CommandTests
    {
        public CommandTests()
        {

        }

        [Fact]
        public void ExceptionCommand_ShouldThrowException()
        {
            var command = new ExceptionCommand();

            var result = () => command.Execute();

            result.Should().Throw<Exception>();
        }

        [Fact]
        public void RepeatExceptionCommand_ShouldThrowException()
        {
            var exc = new ExceptionCommand();
            var command = new RepeatExceptionCommand(exc);

            var result = () => command.Execute();

            result.Should().Throw<Exception>();
        }

        [Fact]
        public void RepeaterCommand_ShouldExecuteProvidedCommand()
        {
            var mockedCommand = new Mock<ICommand>();

            var command = new RepeaterCommand(mockedCommand.Object);

            command.Execute();

            mockedCommand.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void LoggerCommand_ShouldLog()
        {
            var logger = new Mock<ILogger>();

            var command = new LoggerCommand(logger.Object, "some message");

            command.Execute();

            logger.Verify(c => c.Log(It.IsAny<string>()), Times.Once());
        }
    }
}
