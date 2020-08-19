using NHS111.Models.Models.Web.FromExternalServices;
using System;

namespace NHS111.Models.Models.Web
{
    public class ServiceTypeMapping
    {
        private const string CALLBACK_VIEW_NAME = "_callback_type";
        private const string GOTO_VIEW_NAME = "_goto_type";
        private const string PUBLICPHONE_VIEW_NAME = "_phone_type";
        private const string REFERRINGANDGO_VIEW_NAME = "_refer_ring_and_go";
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

                throw new InvalidOperationException("Unknown servicetype with no mapped renderer specified.");
            }
        }
    }


}
