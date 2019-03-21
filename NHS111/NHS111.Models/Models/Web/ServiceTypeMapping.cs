using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web
{
    public class ServiceTypeMapping
    {
        private const string CALLBACK_VIEW_NAME = "_rs_callback_type";
        private const string GOTO_VIEW_NAME = "_rs_goto_type";
        private const string PUBLICPHONE_VIEW_NAME = "_rs_phone_type";
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
                throw new InvalidOperationException("Unknown serivcetype with no mapped renderer specified.");
            }
        }
    }


}
