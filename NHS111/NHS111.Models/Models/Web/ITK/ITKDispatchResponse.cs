using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.ITK
{
    public class ITKDispatchResponse
    {
        private SentStatus _sentStatus = SentStatus.Unsent;
        private string _responseDetail;
        public SentStatus SendSuccess { get { return _sentStatus; } }

        public ITKDispatchResponse()
        {
        }

        public ITKDispatchResponse(SentStatus status)
        {
            _sentStatus = status;
        }
    }

    public enum SentStatus
    {
        Unsent,
        Success,
        Failure
        
    }

}
