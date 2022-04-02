namespace OtusHandlingExeptions.Commands
{
    public class DoubleRepeaterCommand : ICommand
    {
        private readonly ICommand _command;

        public DoubleRepeaterCommand(ICommand command)
        {
            _command = command;
        }

        public void Execute()
        {
            _command.Execute();
        }
    }
}
