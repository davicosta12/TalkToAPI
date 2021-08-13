using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinhasTarefasApi.Helpers.Swagger;
using MinhasTarefasApi.Repositores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkToApi.DataBase;
using TalkToApi.Helpers;
using TalkToApi.Helpers.Constants;
using TalkToApi.V1.Models;
using TalkToApi.V1.Repositores;
using TalkToApi.V1.Repositores.Contracts;
using TalkToApi.V1.Repositories;
using TalkToApi.V1.Repositories.Contracts;

namespace TalkToApi
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
            #region AutoMapper-Config   

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<TalkToContext>(cfg =>
            {
                cfg.UseSqlite("Data Source=DataBase\\TalkTo.db");
            });

            services.AddCors(cfg =>
            {
                cfg.AddDefaultPolicy(policy =>
                {
                    policy
                        .WithOrigins("https://localhost:44389")
                        .AllowAnyMethod()
                        .SetIsOriginAllowedToAllowWildcardSubdomains() // Habilitar CROS para todos os subdomínios
                        .AllowAnyHeader();
                });

                //Habilitar todos os sites, com restrição.
                cfg.AddPolicy("AnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin()
                        .WithMethods("GET")
                        .AllowAnyHeader();
                });
            });

            services.AddScoped<IMensagemRepository, MensagemRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();

            services.AddControllers(cfg =>
            {
                cfg.ReturnHttpNotAcceptable = true; //406
                cfg.InputFormatters.Add(new XmlSerializerInputFormatter(cfg));
                cfg.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                var jsonOutputFormatter = cfg.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add(CustomMediaType.Hateoas);
                }
            })
               .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TalkToContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-api-jwt-minhas-tarefas"))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build()
                );
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddApiVersioning(cfg =>
            {
                cfg.ReportApiVersions = true;
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Adicione o JSON Web Token(JWT) para autenticar",
                    Name = "Authorization"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}
                    }
                });

                c.ResolveConflictingActions(apiDescription => apiDescription.First());
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "TalkToApi - v1.0", Version = "v1.0" });

                var CaminhoProjeto = PlatformServices.Default.Application.ApplicationBasePath;
                var NomeProjeto = $"{PlatformServices.Default.Application.ApplicationName}.xml";
                var CaminhoArquivoXMLComentario = Path.Combine(CaminhoProjeto, NomeProjeto);

                c.IncludeXmlComments(CaminhoArquivoXMLComentario);

                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "TalkToApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseCors("AnyOrigin"); // Desabilite quando for usar Atributos EnableCors/DisableCors

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
