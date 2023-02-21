using Cl9Backup.CLI.Domain.Entities;
using Cl9Backup.CLI.Domain.Persistence;
using LiteDB;

namespace Cl9Backup.CLI.Infrastructure.Persistence
{
    internal class CredencialRepository : Repository<Credencial>, ICredencialRepository
    {
        private const string COLLECTION_NAME = "credentials";
        public CredencialRepository(LiteDatabase db) : base(db, COLLECTION_NAME)
        {
        }

        public bool ExistByName(string name) => Collection.Exists(x => x.Nome == name);
    }
}
