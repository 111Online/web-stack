using RestSharp;
using System;

namespace NHS111.Utils.RestTools
{
    public class JsonRestRequest : RestRequest
    {
        public JsonRestRequest(Method method) : base(method)
        {
        }

        public JsonRestRequest(string resource) : base(resource)
        {
        }

        public JsonRestRequest(Uri resource) : base(resource)
        {
        }

        public JsonRestRequest(string resource, Method method) : base(resource, method)
        {
            this.JsonSerializer = NewtonsoftJsonSerializer.Default;
            OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
        }

        public JsonRestRequest(Uri resource, Method method) : base(resource, method)
        {
            OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
        }
    }
}
