using System;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web
{
    public class ServiceTypeMapping
    {
        private const string CALLBACK_VIEW_NAME = "_rs_callback_type";
        private const string GOTO_VIEW_NAME = "_rs_goto_type";
        private const string PUBLICPHONE_VIEW_NAME = "_rs_phone_type";
        private const string REFERRINGANDGO_VIEW_NAME = "_rs_refer_ring_and_go";
        private const string ONLINE_VIEW_NAME = "_rs_online_type";
        private const string ECONSULT_REFERAL_VIEW_NAME = "_rs_econsult_refearal";
        private const string VIRTUALLY_REFERRAL_VIEW_NAME = "_rs_virtually_referral";
        public ServiceTypeMapping(OnlineDOSServiceType typeGroup)
        {
            _typeGroup = typeGroup;
        }
        private OnlineDOSServiceType _typeGroup;
        public OnlineDOSServiceType ServiceTypeGroup
        {
            get { return _typeGroup; }
        }

        public string TypeViewName
        {
            get
            {
                if (_typeGroup == OnlineDOSServiceType.Callback) return CALLBACK_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.GoTo) return GOTO_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.PublicPhone) return PUBLICPHONE_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.ReferRingAndGo) return REFERRINGANDGO_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.Video) return ONLINE_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.Written) return ONLINE_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.Telephone) return ONLINE_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.EncounterReport) return VIRTUALLY_REFERRAL_VIEW_NAME;
                if (_typeGroup == OnlineDOSServiceType.EConsultReferal) return ECONSULT_REFERAL_VIEW_NAME;

                throw new InvalidOperationException("Unknown servicetype with no mapped renderer specified.");
            }
        }
    }


}
