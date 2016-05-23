namespace NHS111.Web.Presentation.Builders {
    using System.Net.Http;
    using System.Web;

    public abstract class BaseBuilder {
        protected async void CheckResponse(HttpResponseMessage responseMessage) {
            if (responseMessage.IsSuccessStatusCode)
                return;

            throw new HttpException(
                string.Format("There was a problem requesting {0}. {1}",
                    responseMessage.RequestMessage.RequestUri, await responseMessage.Content.ReadAsStringAsync()));
        }
    }
}