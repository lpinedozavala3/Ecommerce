using Database.Models;
using EccomerceAPI.Interfaces;
using EccomerceAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext (usa la clave correcta dentro de ConnectionStrings)
builder.Services.AddDbContext<contextApp>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
const string CorsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, p =>
        p.WithOrigins("http://localhost:4200", "https://localhost:4200")
         .AllowAnyMethod()   // incluye OPTIONS
         .AllowAnyHeader()); // permite X-Store-Domain, etc.
});

// Otros servicios
builder.Services.AddSingleton<IUriService>(o =>
{
    IHttpContextAccessor accessor = o.GetRequiredService<IHttpContextAccessor>();
    HttpRequest request = accessor.HttpContext.Request;
    string uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});
builder.Services.AddTransient<ITenantResolver, TenantResolver>();
builder.Services.AddTransient<ICatalogoService, CatalogoService>();
builder.Services.AddTransient<IAuthService, AuthService>();
    app.UseSwagger();
builder.Services.AddHttpContextAccessor(); // útil si TenantResolver lee headers/host

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ?? CORS debe ir antes de Authorization y antes de MapControllers
app.UseCors(CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
