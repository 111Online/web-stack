using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web
{
    public class AddressSearchViewModel
    {
        public string PostCode { get; set; }
        public List<AddressInfoViewModel> AddressInfoList { get; set; }
        public string SelectedAddress { get; set; }
        public string PostcodeApiAddress { get; set; }
        public string PostcodeApiSubscriptionKey { get; set; }

        public IEnumerable<SelectListItem> SelectListItems
        {
            get
            {
                return AddressInfoList.Select(x => new SelectListItem
                {
                    Value = x.Postcode,
                    Text = string.Format("{0} {1} {2} {3} {4}", x.HouseNumber, x.AddressLine1, x.AddressLine2, x.City, x.Postcode),
                    Selected = false
                });
            }
        }

        public AddressSearchViewModel()
        {
            AddressInfoList = new List<AddressInfoViewModel>();
        }
    }
}