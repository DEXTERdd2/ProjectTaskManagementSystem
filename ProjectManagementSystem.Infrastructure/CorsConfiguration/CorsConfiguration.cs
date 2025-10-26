using Microsoft.Extensions.DependencyInjection;


namespace GamePlanBackend.Infrastructure.CorsConfiguration
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });


            return services;
        }
    }
}
