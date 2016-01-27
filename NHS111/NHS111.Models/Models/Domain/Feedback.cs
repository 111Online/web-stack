using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class Feedback
    {
        public string UserId { get; set; }
        public string JSonData { get; set; }
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public string PageId { get; set; }
        public int? Rating { get; set; }
    }
}
