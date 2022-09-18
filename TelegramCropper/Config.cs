namespace TelegramCropper
{
    public class Config
    {
        public string? ApiKey { get; init; }
        public int MaxTasksLifeTimeSec { get; init; }
        public int MaxTaskProcessTimeoutSec { get; init; }
        public string? HostAddress { get; init; }
    }
}
