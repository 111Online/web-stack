using RestSharp;

namespace NHS111.Web.Presentation.Builders {
    using System.Net.Http;
    using System.Web;

    public abstract class BaseBuilder {
        protected void CheckResponse(IRestResponse response) {
            if (response.IsSuccessful)
                return;

            throw new HttpException(
                string.Format("There was a problem requesting {0}. {1}", response.Request.Resource, response.ErrorMessage));
        }
    }
}