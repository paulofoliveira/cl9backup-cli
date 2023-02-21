using Cl9Backup.CLI.Domain.Entities;
using Cl9Backup.CLI.Domain.Persistence;
using LiteDB;

namespace Cl9Backup.CLI.Infrastructure.Persistence
{
    internal class ParametroRepository : Repository<Parametro>, IParametroRepository
    {
        public ParametroRepository(LiteDatabase db) : base(db, "parametros")
        {
        }

        // TODO: Ser específico e considerar a quantidade de parâmetros necessários.
        public bool IsConfigured() => GetAll().Any();
    }
}
