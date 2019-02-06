using Microsoft.AspNetCore.Mvc;

namespace HdProduction.Dashboard.Api.Controllers
{
  [Route("")]
  public class HomeController
  {
    [HttpGet("")]
    public string Index()
    {
      return "HdProduction.Dashboard.Api is running";
    }
  }
}