using System.Reflection;
using HdProduction.Dashboard.Api.Auth;
using HdProduction.Dashboard.Api.Configuration;
using HdProduction.Dashboard.Application.Queries.Projects;
using HdProduction.Dashboard.Application.Queries.Users;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Infrastructure;
using HdProduction.Dashboard.Infrastructure.NpgsqlOrm;
using HdProduction.Dashboard.Infrastructure.Repositories;
using HdProduction.Dashboard.Infrastructure.Services;
using log4net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace HdProduction.Dashboard.Api
{
    public class Startup
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddApiVersioning();
            services.AddSwaggerGen(c => c.SwaggerDoc("v0", new Info {Title = "HdProduction.Dashboard.Api", Version = "v0"}));

            services.AddJwtAuthentication(Configuration.GetValue<string>("RsaKeysPath:Public"));

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policy.ProjectAdminAccess, policy => policy.Requirements.Add(new ProjectAccessRequirement(ProjectRight.Admin)));
                opt.AddPolicy(Policy.ProjectCreatorAccess, policy => policy.Requirements.Add(new ProjectAccessRequirement(ProjectRight.Creator)));
            });
            services.AddSingleton<IAuthorizationHandler, ProjectAccessRequirementHandler>();

            services.AddHttpContextAccessor();
            services.AddMediatR();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserQuery, UserQuery>();
            services.AddSingleton<ISessionTokenService, JwtTokenService>(c => new JwtTokenService(Configuration.GetValue<string>("RsaKeysPath:Private")));

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectQuery, ProjectQuery>();

            services.AddScoped<IAppBuildQuery, AppBuildQuery>();
            services.AddScoped<IHelpdeskBuildService, HelpdeskBuildService>(c =>
                new HelpdeskBuildService(Configuration.GetValue<string>("HelpdeskHostSources:Path")));

            services.AddSingleton(AutoMapperConfig.Configure());

            services.AddTransient<IDatabaseConnector, DatabaseConnector>(c => new DatabaseConnector(Configuration.GetConnectionString("Db")));
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Db")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Logger.Info("Application starting...");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors(Configuration);
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("v0/swagger.json", "HdProduction.Dashboard.Api"); });
            app.UseAuthentication();
            app.UseMvc();

            Logger.Info("Application is started");
        }
    }
}