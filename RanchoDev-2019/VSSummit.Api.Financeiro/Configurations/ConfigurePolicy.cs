using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace VSSummit.Api.Financeiro.Configurations
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
