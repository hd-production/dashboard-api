using System;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Domain.Entities.Relational;
using HdProduction.Dashboard.Domain.Entities.Users;
using HdProduction.Dashboard.Infrastructure.EfConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HdProduction.Dashboard.Infrastructure
{
  public class ApplicationContext : DbContext, IUnitOfWork
  {
      private readonly IMediator _mediator;

      public ApplicationContext(DbContextOptions<ApplicationContext> opt)
        : base(opt)
      {
      }

      public ApplicationContext(DbContextOptions<ApplicationContext> opt, IMediator mediator)
        : base(opt)
      {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      }

      public DbSet<User> Users { get; set; }
      public DbSet<Project> Projects { get; set; }
      public DbSet<UserProjectRights> UserProjectRights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new UserConfiguration());
      modelBuilder.ApplyConfiguration(new ProjectConfiguration());
      modelBuilder.ApplyConfiguration(new UserProjectRightConfiguration());

      base.OnModelCreating(modelBuilder);
    }
  }
}