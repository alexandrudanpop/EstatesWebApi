namespace WebApp.DAL
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class Repository : IRepository
    {
        private readonly DataBaseContext context;

        public Repository(DataBaseContext context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            this.context.Entry(entity).State = EntityState.Added;
        }

        public void Delete<T>(T entity) where T : class
        {
            this.context.Entry(entity).State = EntityState.Deleted;
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return this.context.Set<T>();
        }
        
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void Update<T>(T entity) where T : class
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }
    }
}