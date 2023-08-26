using System.IO;
using Cms.Data.Repository.Repositories;
using Cms.WebApi.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#pragma warning disable CS1591

[assembly: ApiController]
namespace Cms.WebApi
{
    /// <summary>
    /// 
    /// </summary>
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
            services.AddSingleton<ICmsRepository, InMemoryCmsRepository>();
            services.AddAutoMapper(typeof(CmsMapper));

            services.AddOpenApiDocument(c =>
            {
                c.DocumentName = "v1";
                c.PostProcess = d =>
                {
                    d.Info.Version = "v1";
                    d.Info.Title = "CMS Open API";
                    d.Info.Description = "Open API specification for the CMS";
                    d.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "MIT",
                        Url = "https://opensource.org/license/mit/",
                    };
                    d.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Alex Levinson",
                        Email = "alexander.levinson.70@gmail.com",
                    };
                };
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseOpenApi();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

#pragma warning restore CS1591
