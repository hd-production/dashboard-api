namespace HdProduction.Dashboard.Api.Models.Users
{
  public class UserSignUpRequestModel
  {
    public string Email { get; set; }
    public string PwdHash { get; set; }
  }
}