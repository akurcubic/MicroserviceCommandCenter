using System.Text.Json;
using AutoMapper;
using CommandService.Dtos;
using CommandsService.Data;
using CommandsService.Models;

namespace CommandService.EventProcessing
{

    public class EventProcessor : IEventProcessor
    {

        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        //ne moze da ovde uradimo DI u konstruktoru i da dovucemo repository zato sto ListenerService Singleton i on 
        //funkcionise tokom celog zivota aplikacije i on ce da zove ovaj servis koji ce takodje biti Singleton.
        //da bi ovaj servis ubacio repository kroz di, repository takodje mora da ima lifetime barem kao i ovaj servis.
        //nas repo u program.cs je dodat pomocu addScoped sto znaci da ce za svaki zahtev biti kreirana nova instanca i po zavrsteku zahteva ona ce biti unistena.
        //di radimo dole u klasi

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {   
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType){

                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage){

            Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch(eventType.Event){
                case  "Platform_Published":
                    Console.WriteLine("Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermied;
            }
        }

        private void AddPlatform(string platformPublishedMessage){

            using (var scope = _scopeFactory.CreateScope()){

                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
                

                try{
                    
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExist(plat.ExternalId)){
                        
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Platform added!");

                    }else{
                        Console.WriteLine("--> Platform already exists...");
                    }

                }catch(Exception ex){

                    Console.WriteLine($"--> Could not add platform to Db: {ex.Message}");
                }
            }
        }
    }

    enum EventType{

        PlatformPublished,
        Undetermied
    }
}