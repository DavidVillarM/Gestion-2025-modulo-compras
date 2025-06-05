// Startup.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModuloCompras.Data;
using System.Text.Json.Serialization;

namespace ModuloCompras
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
            => _config = config;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ReactLocalPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")  // Origen de tu front
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers();

            // 2. EF Core con PostgreSQL
            var conn = _config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(o =>
                o.UseNpgsql(conn));

            // 3. Web API + JSON ciclado
            services.AddControllers()
                .AddJsonOptions(o =>
                {
                    // Ignorar referencias circulares al serializar
                    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    o.JsonSerializerOptions.WriteIndented = true;
                });

            // 4. Swagger/OpenAPI
            services.AddSwaggerGen();

            // 5. Para servir SPA en producción
            services.AddSpaStaticFiles(cfg =>
            {
                cfg.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Desarrollo: excepciones y Swagger
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ModuloCompras v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            // Habilitar CORS antes de usar controladores
            app.UseCors("AllowFrontend");
            // ¡Importante! usar CORS **antes** de UseRouting/UseEndpoints
            app.UseCors("ReactLocalPolicy");


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // SPA fallback / proxy en dev
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}

