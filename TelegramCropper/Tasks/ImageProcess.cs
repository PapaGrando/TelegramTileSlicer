using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;
using TelegramCropper.Exceptions;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Tasks
{
    //TODO : Сделать с сохраниние файлов без использования файловой системы
    public class ImageProcess : IDisposable
    {
        private const int _maxTiles = 1000;
        private bool disposedValue;
        private string _tempDir = "TEMP/";
        private string _imgSubFolderName = "tiles";
        private string _zipNameEnds = "out.zip";
        private string _nameId;
        private CancellationToken _cansTok;

        public ImageProcess(string nameId, CancellationToken token)
        {
            _tempDir += nameId + '/';
            _nameId = nameId;
            _cansTok = token;
        }

        public Stream ProcessAction(Stream imageStream, TileTask tiletask,
            IReadOnlyCollection<IFilterTask> filtersQueue)
        {
            imageStream.Position = 0;
            using (var image = Image.Load(imageStream, new PngDecoder()))
            {
                FixTileTask(tiletask);

                if (image.Height < tiletask.Height ||
                    image.Width < tiletask.Width)
                    throw new ChatTaskArgumentsException("Image must be bigger than tile-slice");

                foreach (var action in filtersQueue)
                {
                    if (_cansTok.IsCancellationRequested)
                        throw new ChatTaskTimeoutException();

                    image.Mutate(action.FilterAction);
                }

                var tileDir = $"{_tempDir}{_imgSubFolderName}/";

                if (Directory.Exists(_tempDir))
                    Directory.Delete(_tempDir, true);

                Directory.CreateDirectory(tileDir);
                SliceAndSaveInTempDir(image, _nameId, tileDir, tiletask);
            }
            var distPath = CompressFiles($"{_tempDir}{_imgSubFolderName}/", _tempDir);
            var memStr = new MemoryStream();

            using (var filestr = new FileStream(distPath, FileMode.Open))
                filestr.CopyTo(memStr);

            return memStr;
        }

        private List<(int X, int Y, int Width, int Height)> CalcCropRects(
            Image img,
            (int Height, int Width) sizes)
        {
            var cutPosList = new List<(int X, int Y, int Width, int Height)>();

            for (int x = 0; x <= img.Width - sizes.Width; x += sizes.Width)
                for (int y = 0; y <= img.Height - sizes.Height; y += sizes.Height)
                    cutPosList.Add((x, y, sizes.Width, sizes.Height));

            return cutPosList;
        }

        private void SliceAndSaveInTempDir(Image img, string chatId,
            string dirPath, TileTask tileTask)
        {
            if (tileTask.Sizes == (0, 0))
            {
                img.SaveAsPng($"{dirPath}{chatId}.png");
                return;
            }
            var rects = CalcCropRects(img, tileTask.Sizes);

            if (rects.Count > _maxTiles)
                throw new ChatTaskArgumentsException($"To much tiles - {rects.Count}. Max - {_maxTiles}");

            int iName = 0;
            foreach (var pos in rects)
                using (var tile = img.Clone(x => x.Crop(new Rectangle(pos.X, pos.Y, pos.Width, pos.Height))))
                {
                    if (_cansTok.IsCancellationRequested)
                        throw new ChatTaskTimeoutException();

                    tile.SaveAsPng($"{dirPath}{iName}.png");
                    iName++;
                }

            GC.Collect();
        }

        private TileTask FixTileTask(TileTask tt)
        {
            if (tt.Height == 0 ||
                tt.Width == 0)
            {
                var maxVal = Math.Max(tt.Width, tt.Height);
                tt.Sizes = (maxVal, maxVal);
            }
            return tt;
        }
        private string CompressFiles(string target, string dest)
        {
            if (_cansTok.IsCancellationRequested)
                throw new ChatTaskTimeoutException();

            var filePath = $"{dest}{_zipNameEnds}";
            ZipFile.CreateFromDirectory(target, filePath);
            return filePath;
        }

        #region dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (Directory.Exists(_tempDir))
                    Directory.Delete(_tempDir, true);

                disposedValue = true;
            }
        }

        ~ImageProcess() =>
            Dispose(disposing: false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
