namespace NHS111.Web.Functional.Utils.ScreenShot
{
    public interface IScreenShotMaker
    {
        string BaselineScreenShotDir { get; }
        string ScreenShotDir { get; }
        string ScreenShotUncomparedDir { get; }
        string ScreenShotBaselineDir { get; }
        void MakeScreenShot(string uniqueId, bool uncompared = false);
        bool CheckBaselineExists(string uniqueId);
        bool CheckScreenShotExists(string uniqueId);
        string GetScreenShotFilename(string uniqueId = "1");
        void CopyBaseline(string screenShotFilename);
    }
}
