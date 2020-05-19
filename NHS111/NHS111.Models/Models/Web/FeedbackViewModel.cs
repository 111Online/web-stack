using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace NHS111.Models.Models.Web
{
    public class FeedbackViewModel : TableEntity
    {
        public FeedbackViewModel()
        {
            var now = DateTime.UtcNow;
            PartitionKey = string.Format("{0:yyyy-MM}", now);
            RowKey = string.Format("{0:dd HH-mm-ss-fff}-{1}", now, Guid.NewGuid());
            ShowOnNhsApp = false;
        }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "paragraph")]
        public string Paragraph { get; set; }

        [JsonProperty(PropertyName = "dateAdded")]
        public DateTime DateAdded { get; set; }

        [JsonProperty(PropertyName = "pageId")]
        public string PageId { get; set; }

        [JsonProperty(PropertyName = "showOnNhsApp")]
        public Boolean ShowOnNhsApp { get; set; }

        public PageDataViewModel PageData { get; set; }
    }
}
