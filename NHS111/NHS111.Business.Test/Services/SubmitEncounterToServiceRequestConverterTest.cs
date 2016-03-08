using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Business.ITKDispatcher.Api.Mappings;
using NHS111.Models.Models.Web.ITK;
using NHS111.Web.Presentation.Models;
using NUnit.Framework;
using Address = NHS111.Models.Models.Web.ITK.Address;
using Authentication = NHS111.Models.Models.Web.ITK.Authentication;
using PatientDetails = NHS111.Models.Models.Web.ITK.PatientDetails;

namespace NHS111.Business.Test.Services
{
    public class SubmitEncounterToServiceRequestConverterTest
    {

        public const string TEST_PHONE_NUMBER = "02380666666";
        public const string TEST_PATIENT_FORENAME = "PatientForename";
        public const string TEST_PATIENT_SURNAME = "PatientSurname";

        public const string TEST_PATIENT_HOME_POSTCODE = "HS1 6HH";
        public const string TEST_PATIENT_HOME_STREETADDRESS = "1 home lane";

        public const string TEST_GP_POSTCODE = "TS1 6TH";
        public const string TEST_GP_STREETADDRESS = "1 test gp lane";

        public const string TEST_GP_TELEPHONE = "02380555555";
        public const string TEST_GP_ODS_CODE ="RA286";


        public const int DOS_SERVICE_ID = 1234;
   
   

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
                    Forename = TEST_PATIENT_FORENAME,
                    Surname = TEST_PATIENT_SURNAME,
                    Gender = "Female",
                    GpPractice = new GpPractice()
                    {
                        Address = new Address()
                        {
                            StreetAddressLine1 = TEST_GP_STREETADDRESS,
                            PostalCode = TEST_GP_POSTCODE
                        },
                        Name = "Test GP Practice",
                        ODS = TEST_GP_ODS_CODE,
                        Telephone = TEST_GP_TELEPHONE
                    },
                    HomeAddress = new Address()
                    {
                        StreetAddressLine1 = TEST_PATIENT_HOME_STREETADDRESS,
                        PostalCode = TEST_PATIENT_HOME_POSTCODE
                    },
                    ServiceAddressPostcode = "SV10 6YY",
                    TelephoneNumber = TEST_PHONE_NUMBER
                },
                ServiceDetails = new ServiceDetails()
                {
                    Id = DOS_SERVICE_ID.ToString(),
                    Name = "TestSurgery",
                    PostCode = "TT22 5TT"
                }
            };

            var result = Mapper.Map<ITKDispatchRequest, SubmitEncounterToServiceRequest>(dispatchRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PatientDetails);
            Assert.IsNotNull(result.ServiceDetails);

            Assert.AreEqual(result.PatientDetails.TelephoneNumber, TEST_PHONE_NUMBER);
            Assert.AreEqual(result.PatientDetails.Forename, TEST_PATIENT_FORENAME);
            Assert.AreEqual(result.PatientDetails.Surname, TEST_PATIENT_SURNAME);
            Assert.AreEqual(result.PatientDetails.Gender, gender.Female);

            Assert.AreEqual(result.PatientDetails.HomeAddress.PostalCode, TEST_PATIENT_HOME_POSTCODE );
            Assert.AreEqual(result.PatientDetails.HomeAddress.StreetAddressLine1, TEST_PATIENT_HOME_STREETADDRESS);

            Assert.AreEqual(result.PatientDetails.GpPractice.Address.PostalCode, TEST_GP_POSTCODE);
            Assert.AreEqual(result.PatientDetails.GpPractice.Address.StreetAddressLine1, TEST_GP_STREETADDRESS);

            Assert.AreEqual(result.PatientDetails.GpPractice.Telephone, TEST_GP_TELEPHONE);
            Assert.AreEqual(result.PatientDetails.GpPractice.ODS, TEST_GP_ODS_CODE);

            Assert.AreEqual(result.ServiceDetails.id, DOS_SERVICE_ID);
        }


    }
}
