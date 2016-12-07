using System;
using System.Collections.Generic;
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
                        PostcodeViewModel = new PostcodeViewModel { Postcode = paf.Postcode }
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
                var addressLines = locationResult.AddressLines;
                var addressLine1 = (addressLines == null || addressLines.Length == 0) ? locationResult.StreetDescription : addressLines[0];
                var addressLine2 = (addressLines == null || addressLines.Length < 2) ? string.Empty : addressLines[1];

                var addressInfo = new AddressInfoViewModel()
                {
                    HouseNumber = locationResult.HouseNumber,
                    AddressLine1 = addressLine1,
                    AddressLine2 = addressLine2,
                    City = locationResult.PostTown,
                    County = locationResult.AdministrativeArea,
                    PostcodeViewModel = new PostcodeViewModel { Postcode = locationResult.Postcode },
                    UPRN = locationResult.UPRN
                };

                return addressInfo;
            }
        }
    }
   
}