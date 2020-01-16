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

                // Without this it returns Chrome for Samsung Internet
                if (_request.UserAgent.Contains("SamsungBrowser"))
                {
                    return "Samsung Internet";
                }

                // IE11 on Windows Mobile shows as Mozilla by default
                if (_request.UserAgent.Contains("IEMobile") || browserCapabilities.Browser == "InternetExplorer")
                {
                    return "Internet Explorer";
                }
                
                // On iOS Chrome is Crios in user agent string, without that it shows as Safari
                if (_request.UserAgent.Contains("CriOS"))
                {
                    return "Chrome";
                }

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
                
                
                // Without this it returns a Chrome version number for Samsung Internet
                if (Browser == "Samsung Internet")
                {
                    version = getVersion(_request.UserAgent, "SamsungBrowser");
                }
                
                // On iOS Chrome is Crios in user agent string, without that it shows as Safari
                if (Browser == "Chrome" && Platform == "iOS")
                {
                    version = getVersion(_request.UserAgent, "CriOS");
                }

                // IE11 on Windows Mobile shows as Mozilla by default
                if (Browser == "Internet Explorer" && Platform == "Windows Phone")
                {
                    version = getVersion(_request.UserAgent, "IEMobile");
                }

                // Edge presents as Chrome so needs adjusting to get the real version number
                if (Browser == "Edge")
                {
                    version = getVersion(_request.UserAgent, "Edge");
                }

                return version;
            }
        } 

        public override string Platform
        {
            get
            {
                // Windows 10 (and others?) present as WinNT
                // this changes it to Windows so it makes more sense when reading the data.
                if (_request.Browser.Platform == "WinNT")
                {
                    return "Windows";
                }

                // IE11 on Windows Mobile shows as Mozilla by default
                if (_request.UserAgent.Contains("Windows Phone"))
                {
                    return "Windows Phone";
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

                return  _request.Browser.Platform;
            }
        } 

        public string Referer
        {
            get { 
                var url = _request.UrlReferrer;
                return url != null ? url.AbsoluteUri : string.Empty;
            }
        }

        private string getVersion(string userAgentString, string browser)
        {
            var str = browser + "/";
            var index = userAgentString.IndexOf(str);
            var subStr = userAgentString.Substring(index + str.Length);
            var endIndex = subStr.IndexOfAny(new []{ '.', ' ' });
            return subStr.Substring(0, endIndex); 
        }
    }
}