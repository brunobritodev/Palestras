using Meetup.Api.Financeiro.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Meetup.Api.Financeiro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiSecret = "senha-super-secreta-da-api";
                    options.ApiName = "Meetup-Financeiro";
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Meetup 78 - Api Financeiro ",
                    Description = "Swagger surface",
                    Contact = new Contact { Name = "Bruno Brito", Email = "bhdebrito@gmail.com", Url = "http://www.brunobrito.net.br" },
                    License = new License { Name = "MIT", Url = "https://github.com/brunohbrito/Palestras/blob/master/LICENSE" },

                });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Flow = "implicit",
                    AuthorizationUrl = $"http://localhost:5000/connect/authorize",
                    Scopes = new Dictionary<string, string> {
                        { "meetup-financeiro.leitura", "Leitura CriptoMoedas" },
                        { "meetup-financeiro.escrita", "Acesso Adm" },
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddPolicies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("Development");
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "Api Financeiro");
                c.OAuthClientId("Swagger");
                c.OAuthAppName("User Management UI - full access");
            });
            app.UseMvc();
        }
    }
}
