
using NHS111.Features.Defaults;

namespace NHS111.Features {
    using System.Linq;
    using System.Web;

    public class PostcodePrefillFeature
        : BaseFeature, IPostcodePrefillFeature {

        public PostcodePrefillFeature() {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }

        public bool RequestIncludesPostcode(HttpRequestBase request) {
            return request.QueryString.AllKeys.Contains(_postcodeKeyname);
        }

        public string GetPostcode(HttpRequestBase request) {
            return request.QueryString[_postcodeKeyname];
        }

        private string _postcodeKeyname = "postcode";
    }

    public interface IPostcodePrefillFeature
        : IFeature {
        bool RequestIncludesPostcode(HttpRequestBase request);
        string GetPostcode(HttpRequestBase request);
    }
}