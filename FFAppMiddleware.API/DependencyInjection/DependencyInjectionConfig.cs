using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.Application.Services.Real;
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

            return services;
        }
    }
}
