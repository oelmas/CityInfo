var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();






// Add services to the container.

builder.Services.AddControllers(options =>
{
    // JSON serileştirme ayarları
    options.ReturnHttpNotAcceptable = true; // Acceptable olmayan formatlarda 406 döner
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters(); // XML serileştirme desteği ekler


// OpenAPI desteği için:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// TODO: Mailservice ve Logger service eklenecek.


var app = builder.Build();

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