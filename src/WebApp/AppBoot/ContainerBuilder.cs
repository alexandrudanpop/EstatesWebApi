using DTO.DTO;
using Microsoft.Extensions.DependencyInjection;
using WebApp.DAL;
using WebApp.DAL.DataServices;
using WebApp.DAL.Validators;
using WebApp.Model;

namespace WebApp.AppBoot
{
    public static class ContainerBuilder
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddDbContext<DataBaseContext>();
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IDataService<EstateTempDto>, EstatesDataService>();
            services.AddTransient<IValidator<EstateTempDto>, EstateValidator>();
        }
    }
}