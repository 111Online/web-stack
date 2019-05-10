namespace NHS111.Models.Models.Web.Elements
{
    public class ActionLinkViewModel
    {
        public ActionLinkViewModel()
        {
            Target = "_self";
        }
        public string Text { get; set; }

        public string Url { get; set; }

        public string Target { get; set; }
    }
}
