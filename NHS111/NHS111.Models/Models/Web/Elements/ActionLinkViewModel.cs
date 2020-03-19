namespace NHS111.Models.Models.Web.Elements
{
    public class ActionLinkViewModel
    {
        public ActionLinkViewModel()
        {
            Target = "_self";

            EventTrigger = string.Empty;

            EventValue = string.Empty;
        }
        public string Text { get; set; }

        public string Url { get; set; }

        public string Target { get; set; }

        public string EventTrigger { get; set; }

        public string EventValue { get; set; }
    }
}
