using DukeTransmission.UITests.ExtensionMethods;

namespace DukeTransmission.UITests.TestData
{
    public static class ImageFileManager
    {
        private const string JpgFileName = "JPG_image_1mb.jpg";
        private const string PngFileName = "PNG_image_1mb.png";
        private const string GifFileName = "GIF_image_1mb.gif";

        public static readonly string JpgFilePath = GenerateFilePath(JpgFileName);
        public static readonly string PngFilePath = GenerateFilePath(PngFileName);
        public static readonly string GifFilePath = GenerateFilePath(GifFileName);
        public static string[] ImagePaths = { JpgFilePath, PngFilePath, GifFilePath };

        private static string GenerateFilePath(string fileName)
        {
            return fileName.GenerateFullFilePath();
        }
    }
}
