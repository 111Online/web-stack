using Newtonsoft.Json;
using System.Collections.Generic;

namespace NHS111.Models.Models.Business.Location
{
    public abstract class LocationServiceResultBase<T>
    {
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    public class LocationServiceResult<T> : LocationServiceResultBase<T>
    {
        [JsonProperty(PropertyName = "result")]
        public List<T> Result { get; set; }
    }

    public class LocationServiceSingleResult<T> : LocationServiceResultBase<T>
    {
        [JsonProperty(PropertyName = "result")]
        public T Result { get; set; }
    }

    public class PostcodeLocationResult
    {
        [JsonProperty(PropertyName = "postcode")]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "northings")]
        public int Northings { get; set; }

        [JsonProperty(PropertyName = "eastings")]
        public int Eastings { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }
    }

    public class AddressLocationResult
    {

        [JsonProperty(PropertyName = "udprn")]
        public string Udprn { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "northings")]
        public int Northings { get; set; }

        [JsonProperty(PropertyName = "eastings")]
        public int Eastings { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "post_town")]
        public string Town { get; set; }

        [JsonProperty(PropertyName = "thoroughfare")]
        public string Thoroughfare { get; set; }

        [JsonProperty(PropertyName = "building_number")]
        public string BuildingNumber { get; set; }

        [JsonProperty(PropertyName = "building_name")]
        public string BuildingName { get; set; }

        [JsonProperty(PropertyName = "sub_building_name")]
        public string SubBuildingName { get; set; }

        [JsonProperty(PropertyName = "line_1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "line_2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "line_3")]
        public string AddressLine3 { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "ward")]
        public string Ward { get; set; }



    }

    public class AddressLocationSingleResult
    {
        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("postcode_inward")]
        public string PostcodeInward { get; set; }

        [JsonProperty("postcode_outward")]
        public string PostcodeOutward { get; set; }

        [JsonProperty("post_town")]
        public string PostTown { get; set; }

        [JsonProperty("dependant_locality")]
        public string DependantLocality { get; set; }

        [JsonProperty("double_dependant_locality")]
        public string DoubleDependantLocality { get; set; }

        [JsonProperty("thoroughfare")]
        public string Thoroughfare { get; set; }

        [JsonProperty("dependant_thoroughfare")]
        public string DependantThoroughfare { get; set; }

        [JsonProperty("building_number")]
        public string BuildingNumber { get; set; }

        [JsonProperty("building_name")]
        public string BuildingName { get; set; }

        [JsonProperty("sub_building_name")]
        public string SubBuildingName { get; set; }

        [JsonProperty("po_box")]
        public string PoBox { get; set; }

        [JsonProperty("department_name")]
        public string DepartmentName { get; set; }

        [JsonProperty("organisation_name")]
        public string OrganisationName { get; set; }

        [JsonProperty("udprn")]
        public string Udprn { get; set; }

        [JsonProperty("umprn")]
        public string Umprn { get; set; }

        [JsonProperty("postcode_type")]
        public string PostcodeType { get; set; }

        [JsonProperty("su_organisation_indicator")]
        public string SuOrganisationIndicator { get; set; }

        [JsonProperty("delivery_point_suffix")]
        public string DeliveryPointSuffix { get; set; }

        [JsonProperty("line_1")]
        public string Line1 { get; set; }

        [JsonProperty("line_2")]
        public string Line2 { get; set; }

        [JsonProperty("line_3")]
        public string Line3 { get; set; }

        [JsonProperty("premise")]
        public string Premise { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("administrative_county")]
        public string AdministrativeCounty { get; set; }

        [JsonProperty("postal_county")]
        public string PostalCounty { get; set; }

        [JsonProperty("traditional_county")]
        public string TraditionalCounty { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("ward")]
        public string Ward { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("eastings")]
        public int Eastings { get; set; }

        [JsonProperty("northings")]
        public int Northings { get; set; }
    }
}
