namespace Cl9Backup.CLI.Domain.Persistence
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Delete(int id);
    }
}
