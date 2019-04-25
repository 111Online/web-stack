using System.Configuration;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;

namespace NHS111.Web.Functional.Utils.ScreenShot
{
    public class ScreenShotMaker : IScreenShotMaker
    {
        private readonly IWebDriver _driver;
        
        public ScreenShotMaker(IWebDriver driver)
        {
            _driver = driver;
        }

        public string BaselineScreenShotDir { get
        {
            var baseDirectory = TestContext.CurrentContext.WorkDirectory;
            var directoryInfo = new DirectoryInfo(baseDirectory).Parent;
            if (directoryInfo == null)
                return baseDirectory + "\\" + ConfigurationManager.AppSettings["BaselineScreenShotFolder"] + "\\";

            if (directoryInfo.Parent != null)
                baseDirectory = directoryInfo.Parent.FullName;

            return baseDirectory + "\\" + ConfigurationManager.AppSettings["BaselineScreenShotFolder"] + "\\";
        } }
        public string ScreenShotDir { get { return TestContext.CurrentContext.WorkDirectory + "\\Screenshots\\"; } }
        public string ScreenShotUncomparedDir { get { return ScreenShotDir + "uncompared\\"; } }
        public string ScreenShotBaselineDir { get { return ScreenShotDir + "baselines\\"; } }

        public void MakeScreenShot(string uniqueId, bool uncompared = false)
        {
            var fileName = uncompared ? CreateUncomparedScreenShotFilepath(uniqueId) : CreateScreenShotFilepath(uniqueId);
            var screenshot = _driver.TakeEntireScreenshot();
            _driver.SetCurrentImageUniqueId(uniqueId);
            screenshot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
        }

        public bool CheckBaselineExists(string uniqueId)
        {
            return CheckFileExists(BaselineScreenShotDir + GetScreenShotFilename(uniqueId));
        }

        public bool CheckScreenShotExists(string uniqueId)
        {
            return CheckFileExists(ScreenShotDir + GetScreenShotFilename(uniqueId));
        }

        private bool CheckFileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        public string GetScreenShotFilename(string uniqueId = "1")
        {
            var fileName = string.Format("{0}-{1}.png", TestContext.CurrentContext.Test.FullName, uniqueId);
            return fileName;
        }

        private string CreateBaselineDir()
        {
            Directory.CreateDirectory(ScreenShotBaselineDir);
            return ScreenShotBaselineDir;
        }

        private string CreateUncomparedScreenShotFilepath(string uniqueId)
        {
            return CreateUncomparedScreenShotDir() + GetScreenShotFilename(uniqueId);
        }

        private string CreateUncomparedScreenShotDir()
        {
            Directory.CreateDirectory(ScreenShotUncomparedDir);
            return ScreenShotUncomparedDir;
        }

        private string CreateScreenShotFilepath(string uniqueId)
        {
            return CreateScreenshotDir() + GetScreenShotFilename(uniqueId);
        }

        private string CreateScreenshotDir()
        {
            Directory.CreateDirectory(ScreenShotDir);
            return ScreenShotDir;
        }

        // CopyBaseline copies from the baseline folder on another build to a 
        // baseline folder in its own artifact folder so they are accessible on TeamCity
        public void CopyBaseline(string filename)
        {
            CreateBaselineDir();
            File.Copy(BaselineScreenShotDir + filename, ScreenShotBaselineDir + filename, overwrite: true);
        }
    }
}
