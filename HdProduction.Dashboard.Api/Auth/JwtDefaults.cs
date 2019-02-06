namespace HdProduction.Dashboard.Api.Auth
{
  public static class JwtDefaults
  {
    public const string AuthenticationScheme = "Bearer";
    public const string AuthorizationHeader = "Authorization";
    public const string ClaimsRoleType = "permissions";
    public const string Issuer = "hd-production";
  }
}