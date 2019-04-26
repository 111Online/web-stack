namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class OnlineDOSServiceType
    {
        public readonly bool IsReferral;
        public readonly string ReferralText;

        private OnlineDOSServiceType(string referralText, bool isReferral)
        {
            ReferralText = referralText;
            IsReferral = isReferral;
        }

        public static OnlineDOSServiceType Unknown = new OnlineDOSServiceType(string.Empty, false);
        public static OnlineDOSServiceType Callback = new OnlineDOSServiceType(string.Empty, true);
        public static OnlineDOSServiceType GoTo = new OnlineDOSServiceType("You can go straight to this service. You do not need to telephone beforehand", false);
        public static OnlineDOSServiceType PublicPhone = new OnlineDOSServiceType("You must telephone this service before attending", false);
        public static OnlineDOSServiceType ReferRingAndGo = new OnlineDOSServiceType("This service accepts electronic referrals. You should ring before you go there" , true);
    }
}