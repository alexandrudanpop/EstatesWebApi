namespace Api.DAL
{
    using System;
    using System.Linq;

    public interface IRepository: IDisposable
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        IQueryable<T> GetEntities<T>() where T : class;

        void SaveChanges();

        void Update<T>(T entity) where T : class;
    }
}