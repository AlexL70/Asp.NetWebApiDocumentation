using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cms.Data.Repository.Repositories;
using Cms.WebApi.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.IO;

[assembly: ApiController]
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
