namespace FFAppMiddleware.API.Security
{
    public static class CustomAuthenticationExtension
    {
        public static IServiceCollection AddCustomSecurity(this IServiceCollection services)
        {
            // 1. Înregistrăm Managerul
            services.AddSingleton<ICustomAuthenticationManager, CustomAuthenticationManager>();

            // 2. Înregistrăm Autentificarea
            services.AddAuthentication(CustomBearerAuthenticationOptions.DefaultScheme)
                .AddScheme<CustomBearerAuthenticationOptions, CustomAuthenticationHandler>(CustomBearerAuthenticationOptions.DefaultScheme, null);

            // 3. Înregistrăm Autorizarea (Politicile)
            services.AddAuthorizationBuilder()
                .AddPolicy("RoleAleatorOnly", policy =>
                {
                    policy.RequireRole(nameof(CustomUserRoleEnum.RoleAleator));
                })
                .AddPolicy("ShopifyUserOnly", policy =>
                {
                    policy.RequireRole(nameof(CustomUserRoleEnum.ShopifyUser));
                });

            return services;
        }
    }
}
