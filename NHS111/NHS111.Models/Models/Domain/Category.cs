using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class Category
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
