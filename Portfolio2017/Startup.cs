using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Data;
using Portfolio.Data.Repository;
using System.Net;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Portfolio.API.Core;
using Portfolio.Data.Context;
using Microsoft.Extensions.Logging;
using MySQL.Data.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Portfolio.API.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace Portfolio.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private static IHostingEnvironment _env;
        private static string _applicationPath = string.Empty;
        private string sqlConnectionString = string.Empty;

        /// <summary>
        /// Configuration root
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Entry point into the api
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            _applicationPath = env.WebRootPath;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlConnectionString = Configuration.GetConnectionString("PortfolioDbConn");
            var folderForKeyStore = Configuration["KeyStoreFolderWhichIsBacked"];

            services.AddEntityFrameworkMySQL()
                .AddDbContext<PortfolioContext>(options =>
                {
                    options.UseMySQL(sqlConnectionString);
                });

            // Repositories
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IEmploymentRepository, EmploymentRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<ISkillGroupRepository, SkillGroupRepository>();

            // Enable Cors
            services.AddCors();

            // Add MVC services to the services container.
            services.AddMvc(
                config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                }
                )
                .AddJsonOptions(opts =>
                {
                    // Force Camel Case to JSON
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            //Add the Swagger UI service
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Portfolio API", Version = "v1" });
                //Enable XML comments as part of documentation
                c.IncludeXmlComments(string.Format(@"{0}/bin/Portfolio.API.xml", _applicationPath));
                //Describe all enums as string
                c.DescribeAllEnumsAsStrings();

            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Add Logging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
#if DEBUG
            loggerFactory.AddDebug();
#endif
            
            // Add MVC to the request pipeline.
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            //Add Security Headers
            app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
                .AddDefaultSecurePolicy()
                .AddCustomHeader("Referrer-Policy", "same-origin")
                .AddCustomHeader("X-Pamyb-Security", "PAMYB.org Security. Nothing to see here.")
                );

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
            PortfolioDbInitializer.Initialize(app.ApplicationServices);
        }

        private static RequestDelegate ChangeContextToHttps(RequestDelegate next)
        {
            return async context =>
            {
                context.Request.Scheme = "https";
                await next(context);
            };
        }

    }
    
}