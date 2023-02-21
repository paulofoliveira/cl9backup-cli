using Cl9Backup.CLI.Domain.Entities;

namespace Cl9Backup.CLI.Domain.Persistence
{
    public interface IParametroRepository : IRepository<Parametro>
    {
        bool IsConfigured();
        int ClearCollection();
    }
}
