using AutoMapper;
using Applications.Models;
using Infrastructures;
using Infrastructures.EF.HelpDB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Reflection;
using Infrastructures.AppConfigs.Model;
using Infrastructures.AppConfigs;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel.AspNetCore.AccessTokenValidation;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.Extensions.Primitives;
using Applications.Services;
using static IdentityModel.ClaimComparer;
using Infrastructures.Utilities;

namespace HelpApi
{
    public class Startup
    {
        private AppSetting AppSetting { get; set; }
        private IConfigurationRoot Configuration { get; set; }

        public Startup(AppConfigSetting appConfig)
        {
            AppSetting = appConfig.AppSetting;
            Configuration = appConfig.Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigReadWriteDBContext(services);

            services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            ConfigureBussinessService(services);

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            ConfigSwagger(services);

            ConfigureAuthService(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthClientId("web");
                c.OAuthClientSecret("secretWeb");
                c.OAuthAppName("VERP Swagger UI");
            });

            app.UseHttpsRedirection();

            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void ConfigReadWriteDBContext(IServiceCollection services)
        {
            services.AddDbContext<HelpDBContext>(options =>
            {
                options.UseSqlServer(AppSetting.DatabaseConnections.HelpDatabase);
            });
        }
        private void ConfigureBussinessService(IServiceCollection services)
        {
            services.Configure<AppSetting>(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddScopedServices(typeof(GuideService).Assembly);
        }
        private void ConfigureAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("tokens", p =>
            //    {
            //        p.AddAuthenticationSchemes("introspection");
            //        p.RequireAuthenticatedUser();
            //    });
            //});

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                //.AddPolicyScheme("token", "token", policySchemeOptions =>
                //{
                //    policySchemeOptions.ForwardDefaultSelector = context => "introspection";
                //})
                // JWT tokens (default scheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AppSetting.Authentication.ValidIssuer,

                        ValidateAudience = true,
                        ValidAudience = AppSetting.Authentication.ValidAudience,

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.ExternalHelpApiKey)),

                        ClockSkew = TimeSpan.Zero
                    };

                    // if token does not contain a dot, it is a reference token
                    options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
                    options.RequireHttpsMetadata = false;
                })
                // reference tokens
                .AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = AppSetting.Identity.Endpoint; //https://test-app.verp.vn/endpoint

                    options.ClientId = AppSetting.Identity.ApiName;
                    options.ClientSecret = AppSetting.Identity.ApiSecret;

                    options.TokenRetriever = CustomTokenRetriever.FromHeaderAndQueryString;

                    options.CacheDuration = TimeSpan.FromMinutes(10);
                    options.EnableCaching = true;
                });

            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
        }
        private void ConfigSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "VERP System HTTP API",
                    Version = "v1",
                    Description = "The system Service HTTP API"
                });

                //options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows()
                //    {
                //        Password = new OpenApiOAuthFlow()
                //        {
                //            AuthorizationUrl = new Uri($"{AppSetting.Identity.Endpoint?.TrimEnd('/')}/connect/authorize"),
                //            TokenUrl = new Uri($"{AppSetting.Identity.Endpoint?.TrimEnd('/')}/connect/token"),
                //            Scopes = new Dictionary<string, string>()
                //            {
                //                { "verp offline_access", "verp api common access" }
                //            }
                //        }
                //    },
                //    In = ParameterLocation.Header,
                //    Scheme = "Bearer",
                //    Name = "Authorization",

                //});

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Name = "Authorization",
                    BearerFormat = "Bearer {token}",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""

                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    //{
                    //    new OpenApiSecurityScheme(){ Reference = new OpenApiReference(){ Type = ReferenceType.SecurityScheme, Id="OAuth2" } }, new List<string>()
                    //},
                    {
                        new OpenApiSecurityScheme(){ Reference = new OpenApiReference(){ Type = ReferenceType.SecurityScheme, Id="Bearer" } }, new List<string>()
                    }
                });
            });
        }

        public class CustomTokenRetriever
        {
            internal const string TokenItemsKey = "idsrv4:tokenvalidation:token";
            // custom token key change it to the one you use for sending the access_token to the server
            // during websocket handshake
            internal const string SignalRTokenKey = "signalr_token";

            static Func<HttpRequest, string> AuthHeaderTokenRetriever { get; set; }
            static Func<HttpRequest, string> QueryStringTokenRetriever { get; set; }

            static CustomTokenRetriever()
            {
                AuthHeaderTokenRetriever = TokenRetrieval.FromAuthorizationHeader();
                QueryStringTokenRetriever = TokenRetrieval.FromQueryString();
            }

            public static string FromHeaderAndQueryString(HttpRequest request)
            {
                var token = AuthHeaderTokenRetriever(request);

                if (string.IsNullOrEmpty(token))
                {
                    token = QueryStringTokenRetriever(request);
                }

                if (string.IsNullOrEmpty(token))
                {
                    token = request.HttpContext.Items[TokenItemsKey] as string;
                }

                if (string.IsNullOrEmpty(token) && request.Query.TryGetValue(SignalRTokenKey, out StringValues extract))
                {
                    token = extract.ToString();
                }

                return token;
            }
        }
        
    }
}
