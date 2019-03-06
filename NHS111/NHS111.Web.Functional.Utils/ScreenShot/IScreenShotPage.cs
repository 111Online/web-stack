namespace NHS111.Web.Functional.Utils.ScreenShot
{
    public interface IScreenShotPage
    {
        
        void CompareScreenShot(int uniqueId);
        bool GetScreenShotsEqual();
        T CompareAndVerify<T>(T page, int uniqueId) where T : IScreenShotPage;
        T CompareScreenShot<T>(T page, int uniqueId) where T : IScreenShotPage;
    }
}
