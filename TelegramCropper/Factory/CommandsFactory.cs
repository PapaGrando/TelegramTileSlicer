using TelegramCropper.Commands;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Factory
{
    internal class CommandsFactory
    {
        private IEnumerable<Type> _commandsTypes;
        public CommandsFactory()
        {
            _commandsTypes = typeof(BaseCommand).Assembly
                                            .GetTypes()
                                            .Where(type => type.IsSubclassOf(typeof(BaseCommand)) && !type.IsAbstract)
                                            .ToArray();
        }
        public ICommand? GetCommand(string name)
        {
            foreach (var type in _commandsTypes)
                if (type.Name.ToLower() == name.ToLower())
                    return (ICommand?)Activator.CreateInstance(type);

            return null;
        }
    }
}
