using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

namespace NHS111.Web.Functional.Utils
{
    public static class ScreenshotComparer
    {
        public static bool Compare(string screenshotFilename, string baselineDirectory, string workDirectory)
        {

            using (MagickImage beforeImage = new MagickImage(baselineDirectory + screenshotFilename))

            using (MagickImage afterImage = new MagickImage(workDirectory + screenshotFilename))

            using (MagickImage diffImage = new MagickImage())

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
