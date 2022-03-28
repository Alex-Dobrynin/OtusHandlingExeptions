using OtusHandlingExeptions.Commands;
using OtusHandlingExeptions.Constants;
using OtusHandlingExeptions.Interfaces;

namespace OtusHandlingExeptions.Services
{
    public class ExceptionsHandlerService
    {
        private readonly ILogger _logger;

        private Dictionary<int, Delegate> _handlers { get; } = new();

        public ExceptionsHandlerService(
            ILogger logger)
        {
            _logger = logger;
        }

        private int GetKey<TC, TE>()
        {
            return (typeof(TC), typeof(TE)).GetHashCode();
        }

        private int GetKey(ICommand command, Exception exception)
        {
            return (command.GetType(), exception.GetType()).GetHashCode();
        }

        private Delegate GetHandler<TC, TE>(TC command!!, TE exception!!)
            where TC : ICommand
            where TE : Exception
        {
            var key = GetKey(command, exception).GetHashCode();

            if (_handlers.ContainsKey(key))
            {
                return _handlers[key];
            }
            else
            {
                _logger.Log(string.Format(StringConstants.NoHandler, key));
                return null;
            }
        }

        public void RegisterHandler<TC, TE>(Action<TC, TE> handler!!)
            where TC : ICommand
            where TE : Exception
        {
            var key = GetKey<TC, TE>().GetHashCode();

            if (_handlers.ContainsKey(key)) _handlers[key] = handler;
            else
            {
                _handlers.Add(key, handler);
            }
        }

        public void Handle(ICommand command, Exception exception)
        {
            var handler = GetHandler(command, exception);

            handler?.DynamicInvoke(command, exception);
        }
    }
}
