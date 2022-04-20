namespace TelegramCropper.Commands
{
    public class Help : BaseCommand
    {
        public override string CommandHelp =>
            "Help. How to use :\n\n" +
             "STEP 1) type /new for start\n\n" +
             "STEP 2) (Optional)\n" +
             "Slicing image by tile size\n" +
             "/tile *height *width\n\n" +
             "STEP 2.5) (Optional)\n" +
             "Filters. Available:\n" +
             "/Blackwhite /Sepia /Pixelate /OilPaint /Blur\n" +
             "Applies alternately\n\n" +
             "Use /Status to check added filters and tile size\n\n" +
             "STEP 3) Drop your .png tileset in chat (Without Compression) and Send\n\n" +
             "STEP 4) Wait for Zip archive with .png files\n\n" +
             "Enjoy";
    }
}
