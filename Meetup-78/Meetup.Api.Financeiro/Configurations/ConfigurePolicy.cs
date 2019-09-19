using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Meetup.Api.Financeiro.Configurations
{
    public static class ConfigurePolicy
    {
        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("LeituraCripto", policy => policy.RequireScope("meetup-financeiro.leitura-cripto"));
                options.AddPolicy("AcessoRestrito", policy => policy.RequireScope("meetup-financeiro.acesso-restrito"));
                options.AddPolicy("LeituraDolar", policy => policy.RequireScope("meetup-financeiro.leitura-dolar"));
            });
        }
    }
}
