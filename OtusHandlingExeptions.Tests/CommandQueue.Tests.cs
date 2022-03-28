using System;

using FluentAssertions;

using Moq;

using OtusHandlingExeptions.Commands;
using OtusHandlingExeptions.Services;

using Xunit;

namespace OtusHandlingExeptions.Tests
{
    public class CommandQueueTests
    {
        [Fact]
        public void Enqueue_ShouldThrowException_WhenCommandIsNull()
        {
            var queue = new CommandQueue();

            var result = () => queue.Enqueue(null);

            result.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Dequeue_ShouldReturnNull_WhenQueueIsEmpty()
        {
            var queue = new CommandQueue();

            var command = queue.Dequeue();

            command.Should().BeNull();
        }

        [Fact]
        public void Dequeue_ShouldReturnEnqueuedCommand()
        {
            var queue = new CommandQueue();
            var command = new Mock<ICommand>().Object;

            queue.Enqueue(command);
            var dequeued = queue.Dequeue();

            dequeued.Should().Be(command);
        }

        [Fact]
        public void Dequeue_ShouldReturnCommandAndNull_WhenEnqueueOnce()
        {
            var queue = new CommandQueue();
            var command = new Mock<ICommand>().Object;

            queue.Enqueue(command);

            var first = queue.Dequeue();
            var second = queue.Dequeue();

            first.Should().Be(command);
            second.Should().BeNull();
        }
    }
}
