namespace Api.AppBoot
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsEnvironment("Development"))
            {
                // remove for now
                // builder.AddApplicationInsightsSettings(true);
            }

            builder.AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            AddStaticFilesSettings(app);

            // remove for now because nuget restore failed
            //app.UseApplicationInsightsRequestTelemetry();
            //app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseCors("WebApp");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();

            //services.AddApplicationInsightsTelemetry(this.Configuration);

            services.AddMvcCore().AddJsonFormatters();

            services.AddMvc();
            services.AddSwaggerGen();

            services.AddCors(
                o => o.AddPolicy("WebApp", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

            services.Configure<MvcOptions>(
                options => { options.Filters.Add(new CorsAuthorizationFilterFactory("WebApp")); });

            services.AddOptions();
            services.Configure<AppConfig>(this.Configuration);

            ServiceRegistrator.RegisterServices(services);
        }

        private static void AddStaticFilesSettings(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseStaticFiles(
                new StaticFileOptions
                    {
                        FileProvider =
                            new PhysicalFileProvider(
                                Path.Combine(Directory.GetCurrentDirectory(), "img")),
                        RequestPath = new PathString("/img"),
                        OnPrepareResponse = context =>
                            {
                                var headers = context.Context.Response.GetTypedHeaders();
                                headers.CacheControl = new CacheControlHeaderValue
                                                           {
                                                               MaxAge = TimeSpan.FromDays(7)
                                                           };
                            }
                    });

            app.UseDirectoryBrowser(
                new DirectoryBrowserOptions
                    {
                        FileProvider =
                            new PhysicalFileProvider(
                                Path.Combine(Directory.GetCurrentDirectory(), "img")),
                        RequestPath = new PathString("/img")
                    });
        }
    }
}