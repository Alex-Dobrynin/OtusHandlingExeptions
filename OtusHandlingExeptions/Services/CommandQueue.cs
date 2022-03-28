using OtusHandlingExeptions.Commands;

namespace OtusHandlingExeptions.Services
{
    public class CommandQueue
    {
        private readonly Queue<ICommand> _commandQueue = new();

        public void Enqueue(ICommand command!!)
        {
            _commandQueue.Enqueue(command);
        }

        public ICommand? Dequeue()
        {
            try
            {
                return _commandQueue.Dequeue();
            }
            catch
            {
                return null;
            }
        }
    }
}
