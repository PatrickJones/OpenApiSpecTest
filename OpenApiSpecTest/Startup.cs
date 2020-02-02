using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenApiSpecTest
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
            services.AddControllers();
            services.AddSwaggerGen(d => { d.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Pet Shop", Version = "0.0"}); });
            services.AddSwaggerGen(d => { d.SwaggerDoc("V2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Pet Shop 2", Version = "2.0" }); });
            services.AddSwaggerGen(d => { d.SwaggerDoc("V3", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Pet Shop 3", Version = "3.0" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Swagger")),
                RequestPath = "/swagger/v1/swagger.json"
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet Shop v1"); });
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v2/swagger.json", "Pet Shop v2"); });
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("https://conferenceapi.azurewebsites.net/?format=json", "Pet Shop v3"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}