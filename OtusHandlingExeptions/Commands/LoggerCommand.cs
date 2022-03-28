using OtusHandlingExeptions.Interfaces;

namespace OtusHandlingExeptions.Commands
{
    public class LoggerCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly string _message;

        public LoggerCommand(ILogger logger, string message)
        {
            _logger = logger;
            _message = message;
        }

        public void Execute()
        {
            _logger.Log(_message);
        }
    }
}
