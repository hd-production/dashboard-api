using System;
using System.Collections.Generic;
using HdProduction.Dashboard.Domain.Entities.Relational;
using HdProduction.Dashboard.Domain.Services;

namespace HdProduction.Dashboard.Domain.Entities.Users
{
  public class User : EntityBase<long>
  {
    public User(string email, string firstName, string lastName, string saltedPwd, string pwdSalt)
    {
      Email = email;
      SaltedPwd = saltedPwd;
      PwdSalt = pwdSalt;
      FirstName = firstName;
      LastName = lastName;
      ProjectRights = ProjectRights ?? new List<UserProjectRights>();
    }

    public void SetRefreshToken(string refreshToken)
    {
      if (refreshToken != null && refreshToken.Length != SecurityHelper.RefreshTokenLength)
      {
        throw new ArgumentException("Refresh token length is invalid", nameof(refreshToken));
      }
      RefreshToken = refreshToken;
    }

    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string SaltedPwd { get; }
    public string PwdSalt { get; }
    public string RefreshToken { get; private set; }
    public ICollection<UserProjectRights> ProjectRights { get; } // ef
  }
}