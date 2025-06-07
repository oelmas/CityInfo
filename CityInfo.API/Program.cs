using CityInfo.API;
using CityInfo.API.DbContexts;
using CityInfo.API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AutoMapper;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // JSON serileştirme ayarları
    options.ReturnHttpNotAcceptable = true; // Acceptable olmayan formatlarda 406 döner
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters(); // XML serileştirme desteği ekler

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(
    dbContextOptions => dbContextOptions.UseSqlite(
    builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));
// Detected a TLS handshake to an endpoint that does not have TLS enabled.
// Microsoft.AspNetCore.Server.Kestrel.Core.BadHttpRequestException: Detected a TLS handshake to an endpoint that does not have TLS enabled.
// Kestrel'in HTTPS'i desteklememesi için bu kodu ekledik.

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9200); // HTTP
    options.ListenAnyIP(9201, listenOptions => listenOptions.UseHttps()); // HTTPS
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();