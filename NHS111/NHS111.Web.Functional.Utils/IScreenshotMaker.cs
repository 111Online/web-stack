﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Web.Functional.Utils
{
    public interface IScreenshotMaker
    {
        T MakeScreenshot<T>(T page, string uniqueName, bool uncompared = false) where T : IScreenshotMaker;
        bool CompareScreenshot(string uniqueName);

        T MakeAndCompareScreenshot<T>(T page, string uniqueName) where T : IScreenshotMaker;

        bool GetScreenshotsEqual();
        T CompareAndVerify<T>(ScreenshotComparisonFailureAction action, T page, string uniqueName) where T : IScreenshotMaker;
    }

    public enum ScreenshotComparisonFailureAction
    {
        Fail,
        Pass,
        Warn
    }
}
