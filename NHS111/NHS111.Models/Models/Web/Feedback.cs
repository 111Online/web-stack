using System;

namespace NHS111.Models.Models.Web
{
  public class Feedback
  {
    public string UserId { get; set; }
    public string EmailAddress { get; set; }
    public string JSonData { get; set; }
    public string Text { get; set; }
    public DateTime DateAdded { get; set; }
    public string PageId { get; set; }
    public int? Rating { get; set; }
  }
}