namespace NHS111.Web.Functional.Utils.ScreenShot
{
    public interface IScreenShotMaker
    {
        string BaselineScreenShotDir { get; }
        string ScreenShotDir { get; }
        string ScreenShotUncomparedDir { get; }
        void MakeScreenShot(int uniqueId, bool uncompared = false);
        bool CheckBaselineExists(int uniqueId);
        string GetScreenShotFilename(int uniqueId = 1);
    }
}
