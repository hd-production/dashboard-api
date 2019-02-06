namespace HdProduction.Dashboard.Api.Models
{
  public class AuthenticateRequestModel
  {
    public string Email { get; set; }
    public string PwdHash { get; set; }
  }
}