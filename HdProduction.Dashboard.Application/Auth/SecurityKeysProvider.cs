using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace HdProduction.Dashboard.Application.Auth
{
  public interface ISecurityKeysProvider
  {
    JsonWebKey Public { get; }
    JsonWebKey Private { get; }
  }

  public class SecurityKeysProvider : ISecurityKeysProvider
  {
    public SecurityKeysProvider(string publicKeyPath, string privateKeyPath)
    {
      Public = JsonWebKey.Create(ReadFile(publicKeyPath));
      Private = JsonWebKey.Create(ReadFile(privateKeyPath));
    }

    public JsonWebKey Public { get; }
    public JsonWebKey Private { get; }

    private string ReadFile(string path)
    {
      using (var stream = File.OpenText(path))
      {
        return stream.ReadToEnd();
      }
    }
  }
}