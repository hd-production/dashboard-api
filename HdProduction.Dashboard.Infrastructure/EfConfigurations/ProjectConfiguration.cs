using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Infrastructure.EfConfigurations.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdProduction.Dashboard.Infrastructure.EfConfigurations
{
  public class ProjectConfiguration : EntityBaseConfiguration<Project, long>
  {
    protected override void ConfigureNext(EntityTypeBuilder<Project> builder)
    {
      builder.ToTable(ProjectMetadata.Table);

      builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(128);

      builder.Property(p => p.Status).HasDefaultValue(ProjectStatus.Pending);
      
      builder.Property(p => p.SelfHostSettings)
        .IsRequired(false)
        .HasConversion(new JsonValueConverter<SelfHostSettings>())
        .HasColumnType("Json");
      
      builder.Property(p => p.DefaultAdminSettings)
        .IsRequired(false)
        .HasConversion(new JsonValueConverter<DefaultAdminSettings>())
        .HasColumnType("Json");

      builder.HasMany(p => p.Builds)
        .WithOne()
        .HasForeignKey(pb => pb.ProjectId);

      base.ConfigureNext(builder);
    }
  }
}