using Api.Financeiro.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Financeiro
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

            services.AddMvc();
            services.AddAuthorization();

            services.AddSwagger(Configuration);
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://sso.teste.work"; //Configuration["ApplicationSettings:Authority"]; ;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "invida_api";
                    options.ApiSecret = "*gvwK,34pTR)8*Nr-tWnVd9+nV}:sT=@9=b-k&.FxN*?GX}ob&9hvk@q84vXXE*_";
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
                c.SwaggerEndpoint("./v1/swagger.json", "ID4 API");
                c.OAuthClientId("invida-swagger");
                c.OAuthAppName("Management API");
            });
            app.UseMvc();
        }
    }
}
