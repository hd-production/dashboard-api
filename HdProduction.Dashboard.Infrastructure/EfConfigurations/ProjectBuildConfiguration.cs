using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

namespace HdProduction.Dashboard.Infrastructure.EfConfigurations
{
  public class ProjectBuildConfiguration : IEntityTypeConfiguration<ProjectBuild>
  {
    public void Configure(EntityTypeBuilder<ProjectBuild> builder)
    {
      builder.ToTable(ProjectBuildMetadata.Table);

      builder.HasKey(pb => pb.ProjectId);

      builder.HasOne(pb => pb.Project)
        .WithMany(p => p.Builds)
        .HasForeignKey(pb => pb.ProjectId);

      builder.Property(pb => pb.Status)
        .HasDefaultValue(BuildStatus.InProgress);

      builder.Property(pb => pb.SelfHostConfiguration)
        .IsRequired(false);

      builder.Property(pb => pb.LinkToDownload)
        .IsRequired(false)
        .HasMaxLength(512);

      builder.Property(pb => pb.Error)
        .IsRequired(false)
        .HasColumnType(NpgsqlDbType.Text.ToSql());
    }
  }
}