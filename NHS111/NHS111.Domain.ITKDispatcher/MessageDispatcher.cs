using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Domain.ITKDispatcher.ITKMessageEngineService;

namespace NHS111.Domain.ITKDispatcher
{
    public class MessageDispatcher
    {
        private MessageEngine _itkDispatcher;
        public MessageDispatcher(MessageEngine itkDispatcher)
        {
            _itkDispatcher = itkDispatcher;
        }

        public async void SendITKMessage()
        {
            //_itkDispatcher.SubmitHaSCToServiceAsync(s)
        }
    }
}
