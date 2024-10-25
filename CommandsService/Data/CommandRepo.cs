using CommandsService.Models;

namespace CommandsService.Data
{

    public class CommandRepo : ICommandRepo{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }

        public void CreateCommand(int platformId, Command command)
        {   
            if(command == null){

                throw new System.NotImplementedException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat == null){
                 
                 throw new System.NotImplementedException(nameof(plat));
            }

            _context.Platforms.Add(plat);
        }

        public bool ExternalPlatformExist(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
        #pragma warning disable CS8603 // Possible null reference return.
            return _context.Commands.Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        #pragma warning restore CS8603 // Possible null reference return.
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(c => c.PlatformId == platformId).OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExist(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
            //var platforms = _context.Platforms.ToList();
            //foreach(var plat in platforms){
                //if(plat.Id == platformId){
                    //return true;
               // }
            //}
            //return false;
        }

        public bool SaveChanges()
        {
            return(_context.SaveChanges() >= 0);
        }
    }
}