using System;
using Cms.Data.Repository.Repositories;
using Cms.WebApi.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

#pragma warning disable CS1591
[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Cms.WebApi
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
            services.AddSingleton<ICmsRepository, InMemoryCmsRepository>();
            services.AddAutoMapper(typeof(CmsMapper));
            _ = services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CMS Open API",
                    Version = "v1",
                    Description = "Open API Specificaton for CMS system",
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                    },
                    Contact = new OpenApiContact
                    {
                        Email = "alexander.levinson.70@gmail.com",
                        Name = "Alex Levinson"
                    },
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{nameof(Cms)}.{nameof(WebApi)}.xml");
                s.IncludeXmlComments(xmlPath);
            });

            services.AddControllers(c =>
            {
                //    c.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            });
            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI();

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