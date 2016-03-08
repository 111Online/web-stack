using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Business.ITKDispatcher.Api.Mappings;
using NHS111.Models.Models.Web.ITK;
using NUnit.Framework;
using Address = NHS111.Models.Models.Web.ITK.Address;
using Authentication = NHS111.Models.Models.Web.ITK.Authentication;
using PatientDetails = NHS111.Models.Models.Web.ITK.PatientDetails;

namespace NHS111.Business.Test.Services
{
    public class SubmitEncounterToServiceRequestConverterTest
    {
        [SetUp]
        public void InitilaiseConverters()
        {
            AppDomain.CurrentDomain.GetAssemblies();
            Mapper.AddProfile(new FromItkDispatchRequestToSubmitEncounterToServiceRequest());
        }

        [Test]
        public void FromITKDispatchRequestToSubmitHaSCToService_ConfiguredCorrectly()
        {
            Mapper.AssertConfigurationIsValid("FromItkDispatchRequestToSubmitEncounterToServiceRequest");
        }

        [Test]
        public void Map_ITKDispatchRequest_To_SubmitEncounterToServiceRequest()
        {
            var dispatchRequest = new ITKDispatchRequest()
            {
                Authentication = new Authentication()
                {
                    Password = "testPass",
                    UserName = "testUser"
                },
                PatientDetails = new PatientDetails()
                {
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Forename = "PatientForename",
                    Surname = "PatientSurname",
                    Gender = "Male",
                    GpPractice = new GpPractice()
                    {
                        Address = new Address()
                        {
                            StreetAddressLine1 = "1 test lane",
                            PostalCode = "TS1 6TH"
                        },
                        Name = "Test GP Practice",
                        ODS = "RA286",
                        Telephone = "02380555555"
                    },
                    HomeAddress = new Address()
                    {
                        StreetAddressLine1 = "1 home lane",
                        PostalCode = "HS1 6HH"
                    },
                    ServiceAddressPostcode = "SV10 6YY",
                    TelephoneNumber = "02380666666"
                },
                ServiceDetails = new ServiceDetails()
                {
                    Id = "1234",
                    Name = "TestSurgery",
                    PostCode = "TT22 5TT"
                }
            };

            var result = Mapper.Map<ITKDispatchRequest, SubmitEncounterToServiceRequest>(dispatchRequest);

            Assert.IsNotNull(result);
        }


    }
}
