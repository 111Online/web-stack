using Nest;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(IdProperty = "id", Name = "bodytag")]
    public class BodytagResult
    {

        [Text(Name = "tag")]
        public string Tag { get; set; }
    }
}
