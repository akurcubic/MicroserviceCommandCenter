using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo{

        bool SaveChanges();
        //kada god uradimo neku promenu u bazi moramo da sacuvamo promene

        IEnumerable<Platform> GetAllPlatforms();
        //u C# metode i klase se pisu sa pocetnim velikim slovom
        //IEnumerable predstavlja kolekciju koja se može iterirati, što omogućava povlačenje svih platformi iz baze podataka.

        Platform GetPlatformById(int id);
        void CreatePlatform(Platform plat);


    }
}