using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext{

        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt){


        }
        //Konstruktor klase prima objekat tipa DbContextOptions<AppDbContext> kao parametar, koji sadrži opcije za konfiguraciju baze podataka. Ove opcije se prosleđuju bazičnoj klasi DbContext preko ključne reči base(opt).
        //Ovo omogućava da se kontekst baze podataka konfiguriše spolja, na primer, prilikom podešavanja konekcije na bazu u Startup ili Program fajlu.

        public DbSet<Platform> Platforms { get; set; }
        //DbSet<Platform> predstavlja kolekciju svih entiteta Platform iz baze podataka.
        //Platforms je ime svojstva koje se koristi za interakciju sa tabelom koja sadrži podatke o Platform entitetima.
        //DbSet omogućava CRUD operacije nad entitetima tipa Platform. Svaki entitet u ovom kontekstu će biti mapiran na tabelu u bazi podataka.
        //Ovo polje znaci da ce se prilikom migracije u nasoj bazi pojaviti tabela Platforms

    }

    //Kontekst baze podataka (Database Context) je klasa koja predstavlja vezu između aplikacije i baze podataka u sistemima koji koriste ORM (Object-Relational Mapping) kao što je Entity Framework u .NET-u. Kontekst omogućava aplikaciji da upravlja bazom podataka, šalje upite, čuva, menja i briše podatke bez potrebe za direktnim SQL upitima.
}