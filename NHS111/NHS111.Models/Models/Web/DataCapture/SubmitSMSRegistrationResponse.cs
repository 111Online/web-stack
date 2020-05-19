using RestSharp;
using System.Net;

namespace NHS111.Models.Models.Web.DataCapture
{
    public class SubmitSMSRegistrationResponse
    {
        public SubmitSMSRegistrationResponse(IRestResponse restResponse)
        {
            this.IsSuccessful = restResponse.IsSuccessful;
            if (!IsSuccessful)
            {
                switch (restResponse.StatusCode)
                {
                    case HttpStatusCode.GatewayTimeout:
                        FailState = FailStates.NoResponse;
                        break;
                    case HttpStatusCode.Conflict:
                        FailState = FailStates.TooManyRegistrations;
                        break;

                    case HttpStatusCode.BadRequest:
                        FailState = FailStates.ValidationError;
                        break;
                    default:
                        FailState = FailStates.ServerError;
                        break;
                }
            }
        }
        public bool IsSuccessful { get; set; }

        public FailStates? FailState { get; set; }

    }

    public enum FailStates
    {
        ValidationError,
        TooManyRegistrations,
        ServerError,
        NoResponse
    }
}
