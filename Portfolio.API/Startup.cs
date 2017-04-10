using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MySQL.Data.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portfolio.API.Core;
using Portfolio.Data;
using Portfolio.Data.Context;
using Portfolio.Data.Repository;
using Portfolio.Model.Entities;
using Portfolio.Security;
using Portfolio.Security.Core;
using Swashbuckle.AspNetCore.Swagger;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using Portfolio.XC;

namespace Portfolio.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private static IHostingEnvironment _env;
        private string sqlConnectionString = string.Empty;

        /// <summary>
        /// Configuration root
        /// </summary>
        public IConfigurationRoot _config { get; }

        /// <summary>
        /// Entry point into the api
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _config = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var portfolioConnString = _config.GetConnectionString("PortfolioDbConn");
            var userConnString = _config.GetConnectionString("PortfolioUserDbConn");

            services.AddSingleton(_config);

            //Add the IdentityDbInitializer to Di
            services.AddTransient<PortfolioDbInitializer>();
            services.AddTransient<IdentityDbInitializer>();

            services.AddEntityFrameworkMySQL()
                .AddDbContext<PortfolioContext>(options =>
                {
                    options.UseMySQL(portfolioConnString);
                }, ServiceLifetime.Scoped)
                .AddDbContext<PortfolioIdentityContext>(options =>
                {
                    options.UseMySQL(userConnString);
                }, ServiceLifetime.Scoped);

            services.AddAutoMapper();

            // Add Identity
            services.AddIdentity<PortfolioIdentityUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Lockout = new LockoutOptions
                {
                    AllowedForNewUsers = true,
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30),
                    MaxFailedAccessAttempts = 3
                };
                config.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true,
                    RequireLowercase = true,
                    RequiredLength = 8,
                };
            })
            .AddEntityFrameworkStores<PortfolioIdentityContext>();
            //.AddDefaultTokenProviders();

            // Repositories
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IEmploymentRepository, EmploymentRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<ISkillGroupRepository, SkillGroupRepository>();

            // Enable Cors
            services.AddCors();

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("SuperUsers", p => p.RequireClaim("SuperUser", "True"));
            });

            // Add MVC services to the services container.
            services.AddMvc(
                config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    if (!_env.IsProduction())
                    {
                        config.SslPort = 5000;
                    }
                    config.Filters.Add(new RequireHttpsAttribute());
                }
                )
                .AddJsonOptions(opts =>
                {
                    // Force Camel Case to JSON
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            //Add the Swagger UI service
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Portfolio API", Version = "v1" });
                //Enable XML comments as part of documentation
                c.IncludeXmlComments(string.Format(@"{0}/bin/Portfolio.API.xml", _env.WebRootPath));
                //Describe all enums as string
                c.DescribeAllEnumsAsStrings();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="portfolioSeeder">The portfolio seeder.</param>
        /// <param name="identitySeeder">The identity seeder.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, PortfolioDbInitializer portfolioSeeder, IdentityDbInitializer identitySeeder)
        {
            //Add Logging
            loggerFactory.AddConsole(_config.GetSection("Logging"));
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
            }

            // Add MVC to the request pipeline.
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            //Enable Identity
            app.UseIdentity();

            //Make use of JWT Web tokens
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = _config["Tokens:Issuer"],
                    ValidAudience = _config["Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])),
                    ValidateLifetime = true
                }
            });

            //Add Security Headers
            if (env.IsProduction())
            {
                app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
                .AddDefaultSecurePolicy()
                .AddCustomHeader("Referrer-Policy", "origin")
                .AddCustomHeader("Content-Security-Policy", "default-src https://somedomain.org:*; script-src https://somedomain.org:* 'unsafe-inline'; style-src https://somedomain.org:* 'unsafe-inline'")
                .AddCustomHeader("Public-Key-Pins", "pin-sha256=\"\"; max-age=2592000; includeSubdomains;") //Generate yours here: https://report-uri.io/home/pkp_analyse/
                .AddCustomHeader("X-Additional-Security", "More Security. Nothing to see here.")
                );
            }

            //Add exception handling
            app.UseExceptionHandler(
              builder =>
              {
                  builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
              });
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "ChildApi",
                    template: "api/{parentController}/{parentId}/{controller}/{id}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                //routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            //Add the Swagger UI middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
            });

            //Initialize the database
            portfolioSeeder.Seed();
            identitySeeder.Seed();
        }

    }
}