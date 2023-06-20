using HelpApi.Authorization;
using HelpApi.EF;
using HelpApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<HelpDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("HelpDB"));
});
builder.Services.AddDbContext<MasterDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("MasterDB"));
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddAutoMapper(typeof(Program));

// Life cycle DI
builder.Services.AddScoped<IGuideCateService, GuideCateService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IUserService, UserService>(); 
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
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
    //            AuthorizationUrl = new Uri($"{builder.Configuration["Identity:Endpoint"].TrimEnd('/')}/connect/authorize"),
    //            TokenUrl = new Uri($"{builder.Configuration["Identity:Endpoint"].TrimEnd('/')}/connect/token"),
    //            Scopes = new Dictionary<string, string>()
    //                        {
    //                            { "verp offline_access", "verp api common access" }
    //                        }
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
        //BearerFormat = "Bearer {token}",
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


//Configure Auth
builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

    .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // chỉ yêu cầu HTTPS nếu Production (true) - Development (false)
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                RequireExpirationTime = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                ValidateIssuerSigningKey = true,

            };
        });

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( c =>
    {
            //c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.OAuthClientId("web");
            c.OAuthClientSecret("secretWeb");
            c.OAuthAppName("VERP Swagger UI");
        });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/token", async context =>
    {
        var accessToken = await context.GetTokenAsync("access_token");
        await context.Response.WriteAsync(accessToken);
    });
});

app.Run();
