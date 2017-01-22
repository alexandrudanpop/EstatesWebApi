using System.Linq;
using Microsoft.EntityFrameworkCore;
using Api.Model;

namespace Api.DAL
{
    public class Repository : IRepository
    {
        private readonly DataBaseContext _context;

        public Repository(DataBaseContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return _context.Set<T>();
        }
        
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}