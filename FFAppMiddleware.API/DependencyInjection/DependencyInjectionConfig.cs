using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.Application.Services.Real;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFappMiddleware.DataBase.Repositories.Real;
using FFAppMiddleware.API.Security;

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

            services.AddScoped<ISaleCartService, SaleCartService>();
            services.AddScoped<ISaleCartRepository, SaleCartRepository>();

            services.AddScoped<IAgentManagementService, AgentManagementService>();
            services.AddScoped<IAgentManagementRepository, AgentManagementRepository>();

            #region Security

            services.AddSingleton<ICustomAuthenticationManager, CustomAuthenticationManager>();

            //services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
            //services.AddScoped<ISecurityService, SecurityService>();

            #endregion

            return services;
        }
    }
}
