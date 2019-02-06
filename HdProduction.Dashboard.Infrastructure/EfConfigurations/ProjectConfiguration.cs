using HdProduction.Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdProduction.Dashboard.Infrastructure.EfConfigurations
{
  public class ProjectConfiguration : EntityBaseConfiguration<Project, long>
  {
    protected override void ConfigureNext(EntityTypeBuilder<Project> builder)
    {
      builder.ToTable("project");

      builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(128);

      base.ConfigureNext(builder);
    }
  }
}