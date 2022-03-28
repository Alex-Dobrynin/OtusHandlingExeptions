namespace OtusHandlingExeptions.Commands
{
    public class RepeaterCommand : ICommand
    {
        private readonly ICommand _command;

        public RepeaterCommand(ICommand command)
        {
            _command = command;
        }

        public void Execute()
        {
            _command.Execute();
        }
    }
}
