using ImageMagick;

namespace NHS111.Web.Functional.Utils.ScreenShot
{
    public static class ScreenShotComparer
    {
        public static bool Compare(string screenshotFilename, string baselineDirectory, string workDirectory)
        {

            using (var beforeImage = new MagickImage(baselineDirectory + screenshotFilename))
            using (var afterImage = new MagickImage(workDirectory + screenshotFilename))
            using (var diffImage = new MagickImage())
            {
                var delta = beforeImage.Compare(afterImage, ErrorMetric.Absolute, diffImage);
                var isSame = delta == 0d;
                if (!isSame)
                {
                    // Only write diff screenshot if something has changed
                    // otherwise all files would need to be checked manually (making automated diffing pointless)
                    System.IO.Directory.CreateDirectory(workDirectory + "\\diff\\");
                    diffImage.Write(workDirectory + "\\diff\\" + screenshotFilename);
                }
                return isSame;
            }
        }
    }
}
