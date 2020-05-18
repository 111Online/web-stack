using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NHS111.Utils.Extensions
{
    public static class TaskExtension
    {
        public static async Task<string> AsJson<T>(this Task<T> task)
        {
            return JsonConvert.SerializeObject(await task);
        }

        public static async Task<HttpResponseMessage> AsHttpResponse(this Task<string> task)
        {
            return (await task).AsHttpResponse();
        }

        public static async Task<T> FirstOrDefault<T>(this Task<IEnumerable<T>> task)
        {
            return (await task).FirstOrDefault();
        }

        public static async Task<List<T>> InList<T>(this Task<T> task)
        {
            return (await task).InList();
        }
    }
}