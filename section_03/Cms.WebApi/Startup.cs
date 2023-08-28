using System;
using Cms.Data.Repository.Repositories;
using Cms.WebApi.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;

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
            services.AddControllers(c =>
            {
                //    c.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            });

            services.AddApiVersioning(opt =>
             {
                 opt.AssumeDefaultVersionWhenUnspecified = true;
                 opt.DefaultApiVersion = new ApiVersion(1, 0);
             });
            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'V";
                opt.SubstituteApiVersionInUrl = true;
            });

            var apiVersionDescr = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            _ = services.AddSwaggerGen(s =>
            {
                foreach (var version in apiVersionDescr.ApiVersionDescriptions)
                {
                    s.SwaggerDoc(version.GroupName, new OpenApiInfo
                    {
                        Title = "CMS Open API",
                        Version = version.GroupName,
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
                }
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{nameof(Cms)}.{nameof(WebApi)}.xml");
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescr)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(act =>
            {
                foreach (var version in apiVersionDescr.ApiVersionDescriptions)
                {
                    act.SwaggerEndpoint($"/swagger/{version.GroupName}/swagger.json", version.GroupName);
                }
            });

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