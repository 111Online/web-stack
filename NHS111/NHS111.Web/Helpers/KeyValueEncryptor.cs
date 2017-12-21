using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHS111.Models.Models.Web;

namespace NHS111.Web.Helpers
{
    public static class KeyValueEncryptor
    {
        public static string EncryptedKeys(SearchJourneyViewModel model)
        {
            var encryptor = new QueryStringEncryptor();
            encryptor["sessionId"] = model.SessionId.ToString();
            encryptor["postcode"] = !string.IsNullOrEmpty(model.UserInfo.CurrentAddress.Postcode) ? model.UserInfo.CurrentAddress.Postcode : string.Empty;
            encryptor["searchTerm"] = model.SanitisedSearchTerm;
            encryptor["filterServices"] = model.FilterServices.ToString();
            encryptor["campaign"] = !string.IsNullOrEmpty(model.Campaign) ? model.Campaign : string.Empty;
            encryptor["source"] = !string.IsNullOrEmpty(model.Source) ? model.Source : string.Empty;
            encryptor["digitalTitle"] = !string.IsNullOrEmpty(model.DigitalTitle) ? model.DigitalTitle : string.Empty;

            return encryptor.ToString();
        }
    }
}