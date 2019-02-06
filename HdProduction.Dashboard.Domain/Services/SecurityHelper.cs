using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HdProduction.Dashboard.Domain.Services
{
  public class SecurityHelper
  {
    public const int SaltLength = 32;
    public const int RefreshTokenLength = 44;

    private readonly string _salt;

    public SecurityHelper(string salt)
    {
      if (salt.Length != SaltLength)
      {
        throw new ArgumentException("Salt length is not valid", nameof(salt));
      }
      _salt = salt;
    }

    public static SecurityHelper Create()
    {
      return new SecurityHelper(Guid.NewGuid().ToString("N"));
    }

    public string Salt => _salt;

    public string CreateSaltedPassword(string pwdHash)
    {
      var saltMd5 = HashMd5(_salt);
      var pbkdf2 = new Rfc2898DeriveBytes(pwdHash, saltMd5, 1000);
      var key = pbkdf2.GetBytes(64);
      var hash = BinaryToHex(key);
      return hash;
    }

    public bool CheckPassword(string pwdHash, string saltedPwd)
    {
      return CreateSaltedPassword(pwdHash).SequenceEqual(saltedPwd);
    }

    public string CreateRefreshToken()
    {
      var randomNumber = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
    }

    private static string BinaryToHex(byte[] data)
    {
      var hex = new StringBuilder(data.Length * 2);
      foreach (var b in data)
        hex.AppendFormat("{0:x2}", b);

      return hex.ToString();
    }

    private static byte[] HashMd5(string input)
    {
      var b = Encoding.UTF8.GetBytes(input);
      var md5 = new MD5CryptoServiceProvider();
      var hashData = md5.ComputeHash(b);
      return hashData;
    }
  }
}