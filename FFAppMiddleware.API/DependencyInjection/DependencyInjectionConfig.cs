

using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.Application.Services.Real;
using FFappMiddleware.ApplicationServices.Services.Abstract;
using FFappMiddleware.ApplicationServices.Services.Real;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFappMiddleware.DataBase.Repositories.Real;
using FFAppMiddleware.Model.Repositories.Abstract;
using FFAppMiddleware.Model.Repositories.Real;

namespace FFAppMiddleware.API.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IUserManagementRepository, UserManagementRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
