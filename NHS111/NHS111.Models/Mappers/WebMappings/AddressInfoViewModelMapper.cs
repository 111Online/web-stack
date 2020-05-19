using AutoMapper;
using NHS111.Models.Models.Business.Location;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AddressLocationResult = NHS111.Models.Models.Web.FromExternalServices.IdealPostcodes.AddressLocationResult;


namespace NHS111.Models.Mappers.WebMappings
{
    public class AddressInfoViewModelMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<List<PAF>, List<AddressInfoViewModel>>().ConvertUsing<FromPafToAddressInfoConverter>();
            Mapper.CreateMap<LocationResult, AddressInfoViewModel>().ConvertUsing<FromLocationResultToAddressInfoConverter>();
            Mapper.CreateMap<AddressLocationResult, AddressInfoViewModel>().ConvertUsing<FromPostcodeLocationResultToAddressInfoConverter>();
            Mapper.CreateMap<Models.Business.Location.LocationServiceResult<AddressLocationResult>, AddressInfoCollectionViewModel>().ConvertUsing<FromLocationServiceResultToAddressInfoCollectionConverter>();
            Mapper.CreateMap<AddressLocationSingleResult, AddressInfoViewModel>().ConvertUsing<FromPostcodeLocationSingleResultToAddressInfoConverter>();
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
                else if (tempCity.Trim().Equals(locationResult.AdministrativeArea.Trim()))
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
                    UDPRN = locationResult.UDPRN
                };

                return addressInfo;
            }
        }

        public class FromLocationServiceResultToAddressInfoCollectionConverter : ITypeConverter<Models.Business.Location.LocationServiceResult<AddressLocationResult>, AddressInfoCollectionViewModel>
        {
            public AddressInfoCollectionViewModel Convert(ResolutionContext context)
            {
                var source = (Models.Business.Location.LocationServiceResult<AddressLocationResult>)context.SourceValue;
                var validatedResponse = Models.Web.Validators.PostcodeValidatorResponse.ValidPostcodePathwaysAreaUndefined;
                if (source.Code == "4000" || source.Code == "4001") validatedResponse = Models.Web.Validators.PostcodeValidatorResponse.InvalidSyntax;
                if (source.Code == "4040") validatedResponse = Models.Web.Validators.PostcodeValidatorResponse.PostcodeNotFound;

                return new AddressInfoCollectionViewModel()
                {
                    Addresses = Mapper.Map<List<AddressInfoViewModel>>(source.Result),
                    ValidatedPostcodeResponse = validatedResponse
                };

            }
        }

        public class FromPostcodeLocationResultToAddressInfoConverter : ITypeConverter<AddressLocationResult, AddressInfoViewModel>
        {
            public AddressInfoViewModel Convert(ResolutionContext context)
            {
                var source = (AddressLocationResult)context.SourceValue;

                return new AddressInfoViewModel()
                {
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    AddressLine3 = source.AddressLine3,
                    Thoroughfare = source.Thoroughfare,
                    Ward = source.Ward,
                    City = source.Town,
                    UDPRN = source.Udprn,
                    County = source.County,
                    HouseNumber = String.IsNullOrEmpty(source.BuildingNumber) ? source.BuildingName : source.BuildingNumber,
                    Postcode = source.PostCode
                };
            }
        }

        public class FromPostcodeLocationSingleResultToAddressInfoConverter : ITypeConverter<AddressLocationSingleResult, CurrentAddressViewModel>
        {
            public CurrentAddressViewModel Convert(ResolutionContext context)
            {
                var source = (AddressLocationSingleResult)context.SourceValue;

                return new CurrentAddressViewModel()
                {
                    AddressLine1 = source.Line1,
                    AddressLine2 = source.Line2,
                    AddressLine3 = source.Line3,
                    Thoroughfare = source.Thoroughfare,
                    Ward = source.Ward,
                    City = source.PostTown,
                    UDPRN = source.Udprn,
                    County = source.County,
                    HouseNumber = String.IsNullOrEmpty(source.BuildingNumber) ? source.BuildingName : source.BuildingNumber,
                    Postcode = source.Postcode
                };
            }
        }
    }
}