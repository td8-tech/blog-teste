using Blog.Api.Middleware;
using Blog.Application.Interfaces;
using Blog.Application.Services;
using Blog.Core.Entities;
using Blog.Core.Interfaces;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 1. CONFIGURAÇÃO DE SERVIÇOS

builder.Services.AddControllers();

// Configuração do Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Configuração do ASP.NET Core Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configuração da Autenticação via JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

// Registro dos nossos serviços para Injeção de Dependência
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddSingleton<IWebSocketManager, Blog.Application.Services.WebSocketManager>(); // WebSocketManager como Singleton

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. CONFIGURAÇÃO DO PIPELINE HTTP

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita o suporte a WebSockets
app.UseWebSockets();

// Adiciona o nosso middleware customizado para WebSockets
app.UseMiddleware<WebSocketMiddleware>();

app.UseHttpsRedirection();

// Adiciona os middlewares de Autenticação e Autorização ao pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();