using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace NHS111.Utils.RestTools
{
    public interface IJsonSerializer : ISerializer, IDeserializer
    {

    }
}
