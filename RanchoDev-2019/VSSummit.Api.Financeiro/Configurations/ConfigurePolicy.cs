using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace RanchoDev.Api.Financeiro.Configurations
{
    public static class ConfigurePolicy
    {
        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Leitura", policy => policy.RequireScope("vssummit-financeiro.leitura"));
                options.AddPolicy("Escrita", policy => policy.RequireScope("vssummit-financeiro.escrita"));

            });
        }
    }
}
