using TelegramCropper.Exceptions;

namespace TelegramCropper.Tasks
{
    public class TileTask
    {
        public const int MIN_SLICE_SIZE = 4;
        public const int MAX_SLICE_SIZE = 4096;
        public int Height
        {
            get => _height;
            set
            {
                if (value != 0 &&
                   (value < MIN_SLICE_SIZE ||
                    value > MAX_SLICE_SIZE))
                    throw new TileTaskArgException($"Min: {MIN_SLICE_SIZE}, Max: {MAX_SLICE_SIZE}");

                _height = value;
            }
        }
        public int Width
        {
            get => _width;
            set
            {
                if (value != 0 &&
                   (value < MIN_SLICE_SIZE ||
                    value > MAX_SLICE_SIZE))
                    throw new TileTaskArgException($"Min: {MIN_SLICE_SIZE}, Max: {MAX_SLICE_SIZE}");

                _width = value;
            }
        }
        private int _height;
        private int _width;
        public (int Height, int Width) Sizes
        {
            get => (Height, Width);
            set
            {
                Height = value.Height;
                Width = value.Width;
            }
        }
    }
}
