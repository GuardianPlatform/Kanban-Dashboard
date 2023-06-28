
using FluentValidation;
using Kanban.Dashboard.Api.Middlewares;
using Kanban.Dashboard.Core;
using Kanban.Dashboard.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Kanban.Dashboard.Infrastructure.Extension;
using Kanban.Dashboard.Infrastructure.Seeds;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;

namespace Kanban.Dashboard.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            ConfigureServices(builder.Services, builder.Configuration);


            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        IgnoreSerializableAttribute = true,
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "OpenAPISpecification",
                    new OpenApiInfo()
                    {
                        Title = "Kanban Dashboard WebAPI",
                        Version = "1",
                        Description = "Through this API you can access customer details",
                        License = new OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Bearer xxxxx.yyyyy.zzzzz",
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            await MigrationProvider.Migrate(app.Services);
            await DataSeeder.SeedAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()); 

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureServiceContainer.AddDbContext(services, configuration);
            services.AddAutoMapper(typeof(IApplicationDbContext));
            services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(IApplicationDbContext)));
            services.AddMediatR(x=>x.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(IApplicationDbContext))));
        }
    }
}