namespace NHS111.Web.Functional.Utils.ScreenShot
{
    public interface IScreenShotPage
    {

        void CompareScreenShot(string uniqueId);
        bool GetScreenShotsEqual();
        T CompareAndVerify<T>(T page, string uniqueId) where T : IScreenShotPage;
        T CompareScreenShot<T>(T page, string uniqueId) where T : IScreenShotPage;
    }
}
