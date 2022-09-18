namespace TelegramCropper.Interfaces
{
    public interface ICommandsFactory
    {
        ICommand? GetCommand(string name);
    }
}
