namespace HdProduction.Dashboard.Api.Models
{
  public class UserSignUpRequestModel
  {
    public string Email { get; set; }
    public string PwdHash { get; set; }
  }
}