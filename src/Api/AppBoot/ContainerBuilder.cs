using Api.DAL.DataServices;
using Api.IO;
using Api.Model;
using Api.Validators;
using DTO.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Api.AppBoot
{
    public static class ContainerBuilder
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddTransient<MongoDbContext<Estate>>();
            services.AddTransient<MongoDbContext<Image>>();

            services.AddTransient<IDataService<EstateTempDto>, EstatesDataService>();
            services.AddTransient<IValidator<EstateTempDto>, EstateValidator>();

            services.AddTransient<IDataService<ImageDto>, ImageDataService>();
            services.AddTransient<IValidator<IFormFile>, ImageValidator>();

            services.AddTransient<ImageIoService>();
        }
    }
}