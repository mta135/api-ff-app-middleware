using FFAppMiddleware.Model.Repositories.Abstract;
using FFAppMiddleware.Model.Repositories.Real;

namespace FFAppMiddleware.API.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServicesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUserManagementRepository, UserManagementRepository>();

            return services;
        }
    }
}
