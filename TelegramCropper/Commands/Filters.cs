namespace TelegramCropper.Commands
{
    public class Filters : BaseCommand
    {
        public override string CommandHelp => "Available:\n" +
             "/Blackwhite /Sepia /Pixelate /OilPaint /Blur\n" +
             "Applies alternately";
    }
}