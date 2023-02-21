using Cl9Backup.CLI.Domain.Entities;

namespace Cl9Backup.CLI.Domain.Persistence
{
    public interface ICredencialRepository : IRepository<Credencial>
    {
        bool ExistByName(string name);
    }
}
