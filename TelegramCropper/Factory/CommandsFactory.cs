using TelegramCropper.Commands;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Factory
{
    internal static class CommandsFactory
    {
        private static IEnumerable<Type> _commandsTypesCashing = typeof(BaseCommand).Assembly
                                            .GetTypes()
                                            .Where(type => type.IsSubclassOf(typeof(BaseCommand)) && !type.IsAbstract);
        public static void Cache() => _commandsTypesCashing.FirstOrDefault();
        public static ICommand? GetCommand(string name)
        {
            foreach (var type in _commandsTypesCashing)
                if (type.Name.ToLower() == name.ToLower())
                    return (ICommand?)Activator.CreateInstance(type);

            return null;
        }
    }
}
