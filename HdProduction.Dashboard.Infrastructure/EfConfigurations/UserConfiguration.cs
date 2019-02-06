using HdProduction.Dashboard.Domain.Entities;
using HdProduction.Dashboard.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdProduction.Dashboard.Infrastructure.EfConfigurations
{
  public class UserConfiguration : EntityBaseConfiguration<User, long>
  {
    protected override void ConfigureNext(EntityTypeBuilder<User> builder)
    {
      builder.ToTable("user");

      builder.Property(p => p.Email)
        .IsRequired()
        .HasMaxLength(254);

      builder.Property(p => p.SaltedPwd)
        .IsRequired();

      builder.Property(p => p.PwdSalt)
        .HasMaxLength(SecurityHelper.SaltLength)
        .IsFixedLength();

      builder.Property(p => p.RefreshToken)
        .HasMaxLength(SecurityHelper.RefreshTokenLength)
        .IsFixedLength();

      base.ConfigureNext(builder);
    }
  }
}