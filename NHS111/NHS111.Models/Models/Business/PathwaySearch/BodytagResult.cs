using Nest;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(IdProperty = "id", Name = "bodytag")]
    public class BodytagResult
    {

        [String(Name = "tag", Index = FieldIndexOption.Analyzed)]
        public string Tag { get; set; }
    }
}
