using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public struct OnlineDOSServiceType
    {
        public readonly bool IsReferral;
        public readonly string ReferralText;
        private readonly string _id;

        [JsonProperty(PropertyName = "Id")]
        public string Id {
            get { return _id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this = Unknown;
                }
                else
                {
                    OnlineDOSServiceType type;
                    TypeList.TryGetValue(value, out type);

                    this = type;
                }
            }
        }

        private OnlineDOSServiceType(string id, string referralText, bool isReferral)
        {
            _id = id;
            ReferralText = referralText;
            IsReferral = isReferral;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OnlineDOSServiceType))
                return false;

            var other = (OnlineDOSServiceType)obj;

            return Id == other.Id;
        }

        public static bool operator ==(OnlineDOSServiceType x, OnlineDOSServiceType y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(OnlineDOSServiceType x, OnlineDOSServiceType y)
        {
            return !(x.Equals(y));
        }

        public static OnlineDOSServiceType Unknown = new OnlineDOSServiceType();
        public static OnlineDOSServiceType Callback = new OnlineDOSServiceType("Callback", string.Empty, true);
        public static OnlineDOSServiceType GoTo = new OnlineDOSServiceType("GoTo","You can go straight to this service. You do not need to telephone beforehand", false);
        public static OnlineDOSServiceType PublicPhone = new OnlineDOSServiceType("PublicPhone","You must telephone this service before attending", false);
        public static OnlineDOSServiceType ReferRingAndGo = new OnlineDOSServiceType("ReferRingAndGo","This service accepts electronic referrals. You should ring before you go there" , true);
        public static OnlineDOSServiceType Video = new OnlineDOSServiceType("Onlinevideo", string.Empty, true);
        public static OnlineDOSServiceType Written = new OnlineDOSServiceType("Onlinewritten", string.Empty, true);
        public static OnlineDOSServiceType Telephone = new OnlineDOSServiceType("Onlinetelephone", string.Empty, true);
        public static OnlineDOSServiceType EConsultReferal = new OnlineDOSServiceType("EConsultReferal", string.Empty, true);
        public static Dictionary<string, OnlineDOSServiceType> TypeList = new Dictionary<string, OnlineDOSServiceType>
        {
            { Callback.Id, Callback },
            { GoTo.Id, GoTo },
            { PublicPhone.Id, PublicPhone },
            { ReferRingAndGo.Id, ReferRingAndGo },
            { Video.Id, Video },
            { Written.Id, Written },
            { Telephone.Id, Telephone },
            {EConsultReferal.Id, EConsultReferal }
        };
    }
}