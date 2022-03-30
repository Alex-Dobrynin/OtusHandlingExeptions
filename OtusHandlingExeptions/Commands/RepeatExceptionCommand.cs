namespace OtusHandlingExeptions.Commands
{
    public class RepeatExceptionCommand : ICommand
    {
        private readonly ExceptionCommand _command;

        public RepeatExceptionCommand(ExceptionCommand command)
        {
            _command = command;
        }

        public void Execute()
        {
            _command.Execute();
        }
    }
}
