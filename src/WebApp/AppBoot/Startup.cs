namespace WebApp.AppBoot
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using Model;
    using System.IO;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();

            DbInitializer.Initialize();

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            AddStaticFilesSettings(app);

            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseCors("WebApp");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();

            services.AddApplicationInsightsTelemetry(this.Configuration);

            services.AddMvcCore()
                .AddJsonFormatters();

            services.AddMvc();
            services.AddSwaggerGen();

            services.AddCors(
                o => o.AddPolicy("WebApp", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("WebApp"));
            });

            ContainerBuilder.AddServices(services);
        }

        private static void AddStaticFilesSettings(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/images")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/images")
            });
        }
    }
}