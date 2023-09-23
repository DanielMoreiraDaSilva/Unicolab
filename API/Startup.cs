using System;
using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data.SqlClient;
using Data.Interfaces;
using Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Filters;
using System.Linq;
using Data.Configuration;
using Business.Interfaces;
using Business.Services;
using Business.Mappings;
using Data.Util;
using Business.Hubs;
using Data.Models;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var origins = Configuration.GetSection("Cors")["Origins"].Split(';');
            services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));
            services.AddMvc(options =>
            {
                options.Filters.Add<ApplicationFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "Template padro para APIs .NET Core 3.1",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "API",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                    }
                });
                options.ResolveConflictingActions(x => x.First());
                options.OperationFilter<AddRequiredHeaderParameter>();
            });
            ConfigureDataDependencies(services);
            ConfigureAutoMapper(services);
            ConfigureConnections(services);
            ConfigureAPIServices(services);
            ConfigureOptions(services);
        }

        private void ConfigureConnections(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient(provider => new Func<IDbConnection>(() => new SqlConnection(connectionString)));
        }

        private void ConfigureDataDependencies(IServiceCollection services)
        {
            services.AddTransient<IExportarExcel, ExportarExcel>();
            services.AddTransient<IModuloRepository, ModuloRepository>();
            services.AddTransient<IPerfilModuloRepository, PerfilModuloRepository>();
            services.AddTransient<IPerfilRepository, PerfilRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ILinkImportanteRepository, LinkImportanteRepository>();
            services.AddTransient<IMateriaRepository, MateriaRepository>();
            services.AddTransient<ICursoRepository, CursoRepository>();
            services.AddTransient<IDuvidaRepository, DuvidaRepository>();
            services.AddTransient<IUsuarioMateriaRepository, UsuarioMateriaRepository>();
            services.AddTransient<IOportunidadeRepository, OportunidadeRepository>();
            services.AddTransient<IUsuarioCursoRepository, UsuarioCursoRepository>();
            services.AddTransient<IAreaRepository, AreaRepository>();
            services.AddTransient<IEventoCursoRepository, EventoCursoRepository>();
            services.AddTransient<IOportunidadeCursoRepository, OportunidadeCursoRepository>();
            services.AddTransient<IEventoRepository, EventoRepository>();
            services.AddTransient<IRespostaRepository, RespostaRepository>();
            services.AddTransient<IMateriaCursoRepository, MateriaCursoRepository>();
        }

        private void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddTransient(provider => new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UsuarioMapper());
                mc.AddProfile(new PerfilMapper());
                mc.AddProfile(new PerfilModuloMapper());
                mc.AddProfile(new ModuloMapper());
                mc.AddProfile(new LinkImportanteMapper());
                mc.AddProfile(new MateriaMapper());
                mc.AddProfile(new CursoMapper());
                mc.AddProfile(new UsuarioMateriaMapper());
                mc.AddProfile(new UsuarioCursoMapper());
                mc.AddProfile(new AreaMapper());
                mc.AddProfile(new OportunidadeMapper());
                mc.AddProfile(new DuvidaMapper( provider.GetService<IRespostaRepository>()));
                mc.AddProfile(new EventoCursoMapper());
                mc.AddProfile(new OportunidadeCursoMapper());
                mc.AddProfile(new EventoMapper());
                mc.AddProfile(new RespostaMapper());
            }).CreateMapper());
        }

        private void ConfigureAPIServices(IServiceCollection services)
        {
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IPerfilService, PerfilService>();
            services.AddTransient<ILinkImportanteService, LinkImportanteService>();
            services.AddTransient<IUsuarioMateriaService, UsuarioMateriaService>();
            services.AddTransient<IMateriaService, MateriaService>();
            services.AddTransient<ICursoService, CursoService>();
            services.AddTransient<IDuvidaService, DuvidaService>();
            services.AddTransient<IUsuarioCursoService, UsuarioCursoService>();     
            services.AddTransient<IAreaService, AreaService>();
            services.AddTransient<IOportunidadeService, OportunidadeService>();
            services.AddTransient<IEventoCursoService, EventoCursoService>();
            services.AddTransient<IOportunidadeCursoService, OportunidadeCursoService>();
            services.AddTransient<IEventoService, EventoService>();
            services.AddTransient<IRespostaService, RespostaService>();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<SmtpEmailConfig>(Configuration.GetSection("SmtpEmail"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    //c.RouteTemplate = "/rota/swagger/{documentName}/swagger.json";
                    c.RouteTemplate = "/unicolabapi/swagger/{documentName}/swagger.json";
                });
            }
            else
            {
                app.UseHsts();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
                });
            }
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/rota/swagger/v1/swagger.json",
                //    "API");
                c.SwaggerEndpoint("/unicolabapi/swagger/v1/swagger.json",
                    "API");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ExemploHub>("/exemploHub", o => { o.ApplicationMaxBufferSize = 10000000; o.TransportMaxBufferSize = 10000000; });
            });
        }
    }
}
