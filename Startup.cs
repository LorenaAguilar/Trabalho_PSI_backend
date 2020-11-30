using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SSG_API.Business;
using SSG_API.Data;
using SSG_API.Domain;
using SSG_API.Security;
using SSG_API.Services;
using System;
using System.Linq;

namespace APIProdutos
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";

        private string NormalizeAzureInAppConnString(string raw)
        {
            string conn = string.Empty;
            try
            {
                var dict =
                     raw.Split(';')
                         .Where(kvp => kvp.Contains('='))
                         .Select(kvp => kvp.Split(new char[] { '=' }, 2))
                         .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.InvariantCultureIgnoreCase);
                var ds = dict["Data Source"];
                var dsa = ds.Split(":");
                conn = $"Server={dsa[0]};Port={dsa[1]};Database={dict["Database"]};Uid={dict["User Id"]};Pwd={dict["Password"]};";
            }
            catch
            {
                throw new Exception("unexpected connection string: datasource is empty or null");
            }
            return conn;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PrestadorService>();
            services.AddScoped<ContratanteService>();
            services.AddScoped<ListarServicosService>();
            services.AddScoped<HistoricoDeServicosService>();


            services.AddDbContext<IdentityDbContext>(options =>
            {
                //var conn = Configuration.GetConnectionString("localdb");
                //var conn = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
                options.UseMySql(Configuration.GetConnectionString("MySql"));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //var conn = Configuration.GetConnectionString("localdb");
                //var conn = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
                options.UseMySql(Configuration.GetConnectionString("MySql"));
            });

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de
            // dependências
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            // Configurando a dependência para a classe de validação
            // de credenciais e geração de tokens
            services.AddScoped<AccessManager>();
            
            services.AddSingleton(
                new FacebookConfigurations
                {
                    AcessToken = Configuration.GetConnectionString("FbAccessToken"),
                    AppId = Configuration.GetConnectionString("FbAppId")
                });
            services.AddScoped<FacebookSignInService>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddJwtSecurity(
                signingConfigurations, tokenConfigurations);

            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SSG API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IdentityDbContext appDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSG API V1");
            });

            // Criação de estruturas, usuários e permissões
            // na base do ASP.NET Identity Core (caso ainda não
            // existam)
            new IdentityInitializer(appDbContext, userManager, roleManager)
                .Initialize();

            applicationDbContext.Database.Migrate();


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}