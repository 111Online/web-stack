using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Models.Models.Web.CCG
{
    public class ServiceListModel : List<string>
    {
        public ServiceListModel()
        {
        }
        public ServiceListModel(string serviceidList) : base()
        {
            this.AddRange(serviceidList.Split('|').ToList());
        }
        public override string ToString()
        {
            return String.Join("|", this);
        }
    }
}
