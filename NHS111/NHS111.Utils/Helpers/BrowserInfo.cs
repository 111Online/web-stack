using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHS111.Web.Presentation;
using NHS111.Models.Models.Web;

namespace NHS111.Utils.Helpers
{
    public class BrowserInfo : HttpBrowserCapabilitiesBase
    {
        private readonly HttpRequestBase _request;

        public BrowserInfo(HttpRequestBase request)
        {
            _request = request;
        }

        public string DeviceType
        {
            get
            {
                var deviceType = _request.Browser.IsMobileDevice ? "Mobile" : "Desktop";

                // IsMobileDevice doesn't help with Tablet (considered mobile) so this tries to
                // figure out if its a tablet. Won't be 100% accurate as there is such variety of devices.
                if ((_request.UserAgent.Contains("Android") && !_request.UserAgent.Contains("Mobile"))
                    || _request.UserAgent.Contains("iPad"))
                {
                    deviceType = "Tablet";
                }

                return deviceType;
            }
        }

        public override string Browser
        {
            get { 
                var browserCapabilities = _request.Browser;

                // This fixes an issue where Edge wrongly shows as Chrome in .Browser by default
                if (_request.UserAgent.Contains("Edge") && browserCapabilities.Browser != "Edge")
                {
                    return "Edge";
                }

                return browserCapabilities.Browser;
            }
        }

        public string MajorVersionString
        {
            get
            {
                var version = _request.Browser.MajorVersion.ToString();

                // Edge presents as Chrome so needs adjusting to get the real version number
                if (Browser == "Edge")
                {
                    var str = "Edge/";
                    var index = _request.UserAgent.IndexOf(str);
                    version = _request.UserAgent.Substring(index + str.Length, 2);
                }

                return version;
            }
        } 

        public override string Platform
        {
            get
            {
                var platform = _request.Browser.Platform;

                // Windows 10 (and others?) present as WinNT
                // this changes it to Windows so it makes more sense when reading the data.
                if (_request.Browser.Platform == "WinNT")
                {
                    platform = "Windows";
                }

                // iOS has Mac OS X in userAgent so was showing as Mac
                if (_request.UserAgent.Contains("iPhone") || _request.UserAgent.Contains("iPad"))
                {
                    return "iOS";
                }

                // Mac was being read as Unknown
                if (_request.UserAgent.Contains("Mac OS X") && _request.Browser.Platform == "Unknown")
                {
                    return "Mac";
                }
                
                // Linux was showing as UNIX, which is not ideal
                if (_request.UserAgent.Contains("Linux") && _request.Browser.Platform == "UNIX")
                {
                    return "Linux";
                }

                // Android was being seen as Unknown
                if (_request.UserAgent.Contains("Android") && _request.Browser.Platform == "Unknown")
                {
                    return "Android";
                }

                return platform;
            }
        } 
    }
}