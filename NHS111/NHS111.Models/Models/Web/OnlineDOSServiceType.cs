using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class OnlineDOSServiceType
    {
        [JsonProperty(PropertyName = "isReferral")]
        public bool IsReferral { get; private set;  }

        [JsonProperty(PropertyName = "referralText")]
        public string ReferralText { get; private set; }

        private OnlineDOSServiceType(string referralText, bool isReferral)
        {
            ReferralText = referralText;
            IsReferral = isReferral;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OnlineDOSServiceType))
                return false;

            var other = (OnlineDOSServiceType) obj;

            return IsReferral == other.IsReferral && ReferralText == other.ReferralText;
        }

        public static bool operator ==(OnlineDOSServiceType x, OnlineDOSServiceType y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(OnlineDOSServiceType x, OnlineDOSServiceType y)
        {
            return !(x == y);
        }

        public static OnlineDOSServiceType Unknown = new OnlineDOSServiceType(string.Empty, false);
        public static OnlineDOSServiceType Callback = new OnlineDOSServiceType(string.Empty, true);
        public static OnlineDOSServiceType GoTo = new OnlineDOSServiceType("You can go straight to this service. You do not need to telephone beforehand", false);
        public static OnlineDOSServiceType PublicPhone = new OnlineDOSServiceType("You must telephone this service before attending", false);
        public static OnlineDOSServiceType ReferRingAndGo = new OnlineDOSServiceType("This service accepts electronic referrals. You should ring before you go there" , true);
    }
}