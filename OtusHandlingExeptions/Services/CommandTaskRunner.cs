using OtusHandlingExeptions.Commands;
using OtusHandlingExeptions.Interfaces;

namespace OtusHandlingExeptions.Services
{
    public class CommandTaskRunner : ITaskRunner
    {
        private readonly ILogger _logger;
        private readonly CommandQueue _commandQueue;
        private readonly ExceptionsHandlerService _exceptionsHandlerService;

        public CommandTaskRunner(ILogger logger,
            CommandQueue commandQueue,
            ExceptionsHandlerService exceptionsHandlerService)
        {
            _logger = logger;
            _commandQueue = commandQueue;
            _exceptionsHandlerService = exceptionsHandlerService;
        }

        public void Run()
        {
            while (_commandQueue.Dequeue() is ICommand command)
            {
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    _exceptionsHandlerService.Handle(command, ex);
                }
            }
        }
    }
}
