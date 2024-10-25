using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracija za API Explorer i Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registracija servisa
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// AutoMapper konfiguracija
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dodaj kontrolere
builder.Services.AddControllers();

// HTTP klijent za slanje podataka prema Command servisu
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

// dodajemo nas asihroni klijent koji ce da salje poruke na message bus ali kao singleton
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddGrpc();

//Provera da li je okruženje razvojno i odabir odgovarajuće baze podataka, odnosno DbContexta
if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("--> Using InMemDb");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));
}
else
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}

var app = builder.Build();

// Prikaz okruženja i CommandService URL-a
Console.WriteLine($"--> Trenutno okruženje: {app.Environment.EnvironmentName}");
Console.WriteLine($"--> CommandService URL iz konfiguracije: {builder.Configuration["CommandService"]}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pozivaj kontrolere (omogućava API rute)
app.MapControllers();
//dodajemo grpc servis
app.MapGrpcService<GrpcPlatformService>();
//nije obavezno
app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

// Poziv metode za popunjavanje baze prilikom pokretanja aplikacije
PrepDb.PrepPopulation(app,app.Environment.IsProduction());

// Pokretanje aplikacije
app.Run();
