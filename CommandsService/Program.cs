using CommandService.AsyncDataServices;
using CommandService.EventProcessing;
using CommandsService.Data;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//dodajemo nas DbContext odnosno nasu bazu podataka
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
// Registracija servisa
//Značenje AddScoped
//Scope (Opseg): AddScoped znači da će instanca CommandRepo biti kreirana jednom po HTTP zahtevu. 
//To znači da će se za svaki dolazni HTTP zahtev kreirati nova instanca CommandRepo, koja će biti korišćena tokom cele obrade tog zahteva. Kada se zahtev završi (tj. nakon što se završi obrada odgovora), instanca će biti oslobođena.
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
// AutoMapper konfiguracija, injectuje ga u nas projekat
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor,EventProcessor>();
builder.Services.AddScoped<IPlatformDataClient,PlatformDataClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//pozivamo kontrolere
app.MapControllers(); 

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

PrepDb.PrepPopulation(app);

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
