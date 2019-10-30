using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class LocationResult
    {
        public string[] AddressLines { get; set; }
        public string OrganisationName { get; set; }
        public string BuildingName { get; set; }
        public string UDPRN { get; set; }
        public string StreetDescription { get; set; }
        public string Locality { get; set; }
        public string TownName { get; set; }
        public string AdministrativeArea { get; set; }
        public string PostTown { get; set; }
        public string Postcode { get; set; }
        public string PostcodeLocator { get; set; }
        public LocationCoordinates Coordinate { get; set; }
        public string HouseNumber { get; set; }
        public string GroupDescription { get; set; }
    }
}
