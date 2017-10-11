using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Mappers.WebMappings
{
    public class AddressInfoViewModelMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<List<PAF>, List<AddressInfoViewModel>>().ConvertUsing<FromPafToAddressInfoConverter>();
            Mapper.CreateMap<LocationResult, AddressInfoViewModel>().ConvertUsing<FromLocationResultToAddressInfoConverter>();
        }

        public class FromPafToAddressInfoConverter : ITypeConverter<List<PAF>, List<AddressInfoViewModel>>
        {
            public List<AddressInfoViewModel> Convert(ResolutionContext context)
            {
                var pafList = (List<PAF>)context.SourceValue;
                var listAddressInfo = new List<AddressInfoViewModel>();
                foreach (var paf in pafList)
                {

                    listAddressInfo.Add(new AddressInfoViewModel
                    {
                        HouseNumber = paf.BuildingName,
                        AddressLine1 = paf.BuildingNumber,
                        AddressLine2 = paf.DepartmentName,
                        City = paf.Town,
                        Postcode = paf.Postcode
                    });
                }

                return listAddressInfo;
            }
        }

        public class FromLocationResultToAddressInfoConverter : ITypeConverter<LocationResult, AddressInfoViewModel>
        {
            public AddressInfoViewModel Convert(ResolutionContext context)
            {
                var locationResult = (LocationResult)context.SourceValue;
                string city;
                string county;
                var tempCity = string.IsNullOrEmpty(locationResult.PostTown) ? locationResult.Locality : locationResult.PostTown;

                if (string.IsNullOrEmpty(tempCity))
                {
                    city = locationResult.AdministrativeArea;
                    county = string.Empty;
                }
                else if(tempCity.Trim().Equals(locationResult.AdministrativeArea.Trim()))
                {
                    city = tempCity;
                    county = string.Empty;
                }
                else
                {
                    city = tempCity;
                    county = locationResult.AdministrativeArea;
                }

                var addressLines = locationResult.AddressLines;

                string addressLine1;
                string addressLine2;

                if (!string.IsNullOrEmpty(locationResult.BuildingName))
                {
                    if (Regex.IsMatch(locationResult.BuildingName, "^[0-9]{1,}[a-zA-Z0-9\\s-]*$"))
                    {
                        //building name actually stores a house number in the format 7A, 58C or similar
                        addressLine1 = string.Format("{0} {1}", locationResult.BuildingName, locationResult.StreetDescription);
                        addressLine2 = string.Empty;
                    }
                    else
                    {
                        addressLine1 = locationResult.BuildingName;
                        addressLine2 = locationResult.StreetDescription;    
                    }
                }
                else
                {
                    addressLine1 = (addressLines == null || addressLines.Length == 0) ? locationResult.StreetDescription : addressLines[0];
                    addressLine2 = (addressLines == null || addressLines.Length < 2) ? string.Empty : addressLines[1];

                    if (addressLine2.Trim().Equals(city.Trim()))
                    {
                        addressLine2 = string.Empty;
                    }
                }

                var addressInfo = new AddressInfoViewModel()
                {
                    HouseNumber = locationResult.HouseNumber,
                    AddressLine1 = addressLine1,
                    AddressLine2 = addressLine2,
                    City = city,
                    County = county,
                    Postcode = locationResult.Postcode,
                    UPRN = locationResult.UPRN
                };

                return addressInfo;
            }
        }
    }
}