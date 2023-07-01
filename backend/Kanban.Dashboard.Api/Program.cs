
using FluentValidation;
using Kanban.Dashboard.Api.Middlewares;
using Kanban.Dashboard.Core;
using Kanban.Dashboard.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;
using Kanban.Dashboard.Core.Entities;
using Kanban.Dashboard.Infrastructure.Extension;
using Kanban.Dashboard.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Kanban.Dashboard.Core.Services;
using Kanban.Dashboard.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
                    "v1",
                    new OpenApiInfo()
                    {
                        Title = "Kanban Dashboard WebAPI",
                        Version = "1",
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
                    Description = "Authorize using bearer token...",
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
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Kanban Dashboard API");
                setupAction.RoutePrefix = "swagger";
            });

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureServiceContainer.AddDbContext(services, configuration);

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(o =>
             {
                 o.RequireHttpsMetadata = false;
                 o.SaveToken = false;
                 o.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.Zero,
                     ValidIssuer = configuration["JWTSettings:Issuer"],
                     ValidAudience = configuration["JWTSettings:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                 };
                 o.Events = new JwtBearerEvents()
                 {
                     OnAuthenticationFailed = c =>
                     {
                         c.NoResult();
                         c.Response.StatusCode = 500;
                         c.Response.ContentType = "text/plain";
                         return c.Response.WriteAsync(c.Exception.ToString());
                     },
                     OnChallenge = context =>
                     {
                         context.HandleResponse();
                         context.Response.StatusCode = 401;
                         context.Response.ContentType = "application/json";
                         var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                         return context.Response.WriteAsync(result);
                     },
                     OnForbidden = context =>
                     {
                         context.Response.StatusCode = 403;
                         context.Response.ContentType = "application/json";
                         var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                         return context.Response.WriteAsync(result);
                     },
                 };
             });

            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder =>
                    builder.SetIsOriginAllowed(origin => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                ));

            services.AddTransient<IAccountService, AccountService>();
            services.Configure<JwtSettings>(configuration.GetSection("JWTSettings"));

            services.AddAutoMapper(typeof(IApplicationDbContext));
            services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(IApplicationDbContext)));
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(IApplicationDbContext))));
        }
    }
}