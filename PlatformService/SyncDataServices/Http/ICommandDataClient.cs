using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }

    //Task je ključna klasa u .NET-u koja se koristi za rad sa asinkronim operacijama. Ona predstavlja operaciju koja se izvršava u pozadini i omogućava kodu da nastavi s radom dok se ta operacija ne završi.
}
