
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace NHS111.Web.Functional.Utils {
    public static class IWebDriverExtensions
    {
        private static string _currentImageUniqueId = "1";

        public static bool ElementExists(this IWebDriver driver, By by) {
            return driver.FindElements(by).Any();
        }

        public static bool IfElementExists(this IWebDriver driver, By by, Action<IWebElement> action) {
            var elements = driver.FindElements(by);
            if (!elements.Any())
                return false;

            action(elements.First());

            return true;
        }

        public static IWebElement TabFrom(this IWebDriver driver, IWebElement element) {
            element.SendKeys(Keys.Tab);
            return driver.SwitchTo().ActiveElement();
        }

        public static string GetCurrentImageUniqueId(this IWebDriver driver)
        {
            return _currentImageUniqueId;
        }

        public static void SetCurrentImageUniqueId(this IWebDriver driver, string uniqueId)
        {
            _currentImageUniqueId = uniqueId;
        }

        public static Screenshot TakeEntireScreenshot(this IWebDriver driver)
        {
            // Make sure every screenshot starts at the top of the page
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,0)");

            // Get the total size of the page
            var totalWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.offsetWidth"); //documentElement.scrollWidth");
            var totalHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return  document.body.parentNode.scrollHeight");
            
            // Get the size of the viewport
            var viewportWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.clientWidth"); //documentElement.scrollWidth");
            var viewportHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight"); //documentElement.scrollWidth");

            // We only care about taking multiple images together if it doesn't already fit
            if (totalWidth <= viewportWidth && totalHeight <= viewportHeight)
            {
                var screenshot = driver.TakeScreenshot();
                return screenshot;
            }
            // Split the screen in multiple Rectangles
            var rectangles = new List<Rectangle>();
            // Loop until the totalHeight is reached
            for (var y = 0; y < totalHeight; y += viewportHeight)
            {
                var newHeight = viewportHeight;
                // Fix if the height of the element is too big
                if (y + viewportHeight > totalHeight)
                {
                    newHeight = totalHeight - y;
                }
                // Loop until the totalWidth is reached
                for (var x = 0; x < totalWidth; x += viewportWidth)
                {
                    var newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (x + viewportWidth > totalWidth)
                    {
                        newWidth = totalWidth - x;
                    }
                    // Create and add the Rectangle
                    var currRect = new Rectangle(x, y, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }
            // Build the Image
            var stitchedImage = new Bitmap(totalWidth, totalHeight);
            // Get all Screenshots and stitch them together
            var previous = Rectangle.Empty;
            foreach (var rectangle in rectangles)
            {
                // Calculate the scrolling (if needed)
                if (previous != Rectangle.Empty)
                {
                    var xDiff = rectangle.Right - previous.Right;
                    var yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                }
                // Take Screenshot
                var screenshot = driver.TakeScreenshot();
                // Build an Image out of the Screenshot
                var screenshotImage = ScreenshotToImage(screenshot);
                // Calculate the source Rectangle
                var sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);
                // Copy the Image
                using (var graphics = Graphics.FromImage(stitchedImage))
                {
                    graphics.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                }
                // Set the Previous Rectangle
                previous = rectangle;
            }

            return ImageToScreenshot(stitchedImage);
        }

        private static Image ScreenshotToImage(Screenshot screenshot)
        {
            Image screenshotImage;
            using (var memStream = new MemoryStream(screenshot.AsByteArray))
            {
                screenshotImage = Image.FromStream(memStream);
            }

            return screenshotImage;
        }

        private static Screenshot ImageToScreenshot(Image image)
        {

            using (var memStream = new MemoryStream())
            {
                image.Save(memStream, ImageFormat.Png);

                return new Screenshot(Convert.ToBase64String(memStream.ToArray()));
            }
        }
    }
}
