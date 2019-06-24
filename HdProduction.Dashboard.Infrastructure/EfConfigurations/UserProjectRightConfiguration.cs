using HdProduction.Dashboard.Domain.Entities.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdProduction.Dashboard.Infrastructure.EfConfigurations
{
  public class UserProjectRightConfiguration : IEntityTypeConfiguration<UserProjectRights>
  {

    public void Configure(EntityTypeBuilder<UserProjectRights> builder)
    {
      builder.ToTable(UserProjectRightsMetadata.Table);

      builder.HasKey(up => new {up.UserId, up.ProjectId});

      builder.HasOne(up => up.User)
        .WithMany(u => u.ProjectRights)
        .HasForeignKey(up => up.UserId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Property(up => up.Right)
        .IsRequired();
    }
  }
}