using Newtonsoft.Json;
using RestSharp.Serializers;
using System.IO;

namespace NHS111.Utils.RestTools
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private Newtonsoft.Json.JsonSerializer serializer;

        public NewtonsoftJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public string ContentType
        {
            get { return "application/json"; }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    try
                    {
                        return serializer.Deserialize<T>(jsonTextReader);
                    }
                    catch (JsonReaderException e)
                    {
                        throw new JsonReaderException(string.Format("Problem deserialising the following {0} response from {1}, (see inner exception for further details): {2}", response.StatusCode, response.ResponseUri.OriginalString, content), e);
                    }
                }
            }
        }

        public static NewtonsoftJsonSerializer Default
        {
            get
            {
                return new NewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });
            }
        }

        string ISerializer.ContentType
        {
            get { return "application/json"; }
            set { }
        }
    }
}
