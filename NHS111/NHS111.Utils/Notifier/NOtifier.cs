using System.Net.Http;
using System.Text;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;

namespace NHS111.Utils.Notifier
{
    public class Notifier : INotifier<string>
    {
        private readonly IRestfulHelper _restfulHelper;

        public Notifier(IRestfulHelper restfulHelper)
        {
            _restfulHelper = restfulHelper;
        }

        public void Notify(string url, string id)
        {
            var request = new HttpRequestMessage { Content = new StringContent(id, Encoding.UTF8, "application/json") };
            _restfulHelper.PostAsync(url, request);
        }
    }

    public interface INotifier<in T>
    {
        void Notify(T id, string url);
    }
}