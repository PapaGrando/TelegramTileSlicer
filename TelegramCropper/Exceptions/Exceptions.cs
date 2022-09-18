namespace TelegramCropper.Exceptions
{
    /// <summary>
    /// Base class for trapping user errors. m should have a message to send to the user
    /// </summary>
    public abstract class BaseCommandException : Exception
    {
        public BaseCommandException(string m) : base(m) { }
    }
    public abstract class ChatTaskException : Exception
    {
        public ChatTaskException(string m) : base(m) { }
    }

    public class TileTaskArgException : Exception
    {
        public TileTaskArgException(string m) : base(m) { }
    }

    public class CommandArgumentsException : BaseCommandException
    {
        public CommandArgumentsException(string mes) : base(mes) { }
    }

    public class ChatTaskArgumentsException : ChatTaskException
    {
        public ChatTaskArgumentsException(string m) : base(m) { }
    }

    public class ChatTaskBusyException : ChatTaskException
    {
        public ChatTaskBusyException() : base("Task is Running. Wait...") { }
    }
    public class ChatTaskTimeoutException : ChatTaskException
    {
        public ChatTaskTimeoutException() : base("Task timeout") { }
    }
}
