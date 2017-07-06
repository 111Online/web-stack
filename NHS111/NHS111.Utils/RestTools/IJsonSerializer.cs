using Neo4jClient.Serialization;
using RestSharp.Deserializers;

namespace NHS111.Utils.RestTools
{
    public interface IJsonSerializer : ISerializer, IDeserializer
    {

    }
}
