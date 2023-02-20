using Cl9Backup.CLI.Domain.Persistence;
using LiteDB;

namespace Cl9Backup.CLI.Infrastructure.Persistence
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly LiteDatabase _db;
        private readonly string _collectionName;
        public Repository(LiteDatabase db, string collectionName)
        {
            _db = db;
            _collectionName = collectionName;
        }
        public void Add(T entity) => Collection.Insert(entity);
        public void Delete(int id) => Collection.Delete(new BsonValue(id));
        public IEnumerable<T> GetAll() => Collection.FindAll();
        public T GetById(int id)
        {
            if (id <= 0)
                return default!;

            var item = Collection.FindById(new BsonValue(id));

            return item;
        }
        public void Update(T entity) => Collection.Update(entity);
        protected ILiteCollection<T> Collection => _db.GetCollection<T>(_collectionName);
    }
}
