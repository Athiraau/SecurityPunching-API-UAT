
using DataAccess.Repository;
using DataAccess.Contracts;
using DataAccess.Entities;
using NLog.Extensions.Logging;
using SecurityPunching.Extensions;
using DataAccess.Context;
using Business.Contracts;
using Business.Services;
using DataAccess.Dto;
using Business.Services.GetPunch;
using DataAccess.Dto.Request;
using FluentValidation;
using SecurityPunching.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Business.Helpers;

var builder = WebApplication.CreateBuilder(args);

var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);

builder.Logging.AddNLog(logPath).SetMinimumLevel(LogLevel.Trace);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddTransient<DapperContext>();
builder.Services.AddTransient<IServiceWrapper, ServiceWrapper>();
builder.Services.AddTransient<SecurityKeyPunching>();
builder.Services.AddTransient<DtoWrapper>();
builder.Services.AddTransient<ServiceHelper>();
builder.Services.AddTransient<ErrorResponse>();
builder.Services.AddTransient<ISecurityPunchService,SecurityPunchService>();
builder.Services.AddTransient<SecPunchReqDto>();
builder.Services.AddTransient<IValidator<SecPunchReqDto>,SecPunchValidator>();
builder.Services.AddTransient<SecPunchImgPostDto>();
builder.Services.AddTransient<IValidator<SecPunchPostDto>, UpdateImageValidator>();
builder.Services.AddTransient<ImgValidationHelper>();

builder.Services.Configure<IISOptions>(options =>
{
    options.AutomaticAuthentication = false;
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(x =>
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthentication(x =>
{
x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
o.SaveToken = true;
o.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],
    ValidAudience = builder.Configuration["JWT:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Key)
};
});

// Add Cors
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed((host) => true);
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<CorsMiddleware>();

app.UseMiddleware<JwtMiddlewareExtension>();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
