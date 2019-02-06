using HdProduction.Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdProduction.Dashboard.Infrastructure.EfConfigurations
{
  public class EntityBaseConfiguration<TEntity, T> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase<T>
    where T : struct
  {
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
      builder.HasKey(p => p.Id);

      builder.Property(p => p.Id)
        .ValueGeneratedOnAdd();

      ConfigureNext(builder);
    }

    protected virtual void ConfigureNext(EntityTypeBuilder<TEntity> builder)
    {
    }
  }
}