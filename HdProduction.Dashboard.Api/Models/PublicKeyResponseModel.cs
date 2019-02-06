using Newtonsoft.Json;

namespace HdProduction.Dashboard.Api.Models
{
  public class PublicKeyResponseModel
  {
    [JsonProperty("alg")]
    public string Algorithm { get; set; }
    [JsonProperty("use")]
    public string Use { get; set; }
    [JsonProperty("e")]
    public string Exponent { get; set; }
    [JsonProperty("kty")]
    public string KeyType { get; set; }
    [JsonProperty("n")]
    public string Modulus { get; set; }
  }
}