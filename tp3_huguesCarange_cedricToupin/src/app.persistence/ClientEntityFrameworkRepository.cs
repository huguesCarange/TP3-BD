using System;
using System.Collections.Generic;
using System.Linq;
using app.domain;
using Microsoft.EntityFrameworkCore;

namespace app.persistence
{
    public class ClientEntityFrameworkRepository<T> : IArtisteRepository<T> where T : Entity
    {
        private readonly DbContext _context;

        public ClientEntityFrameworkRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
    }
}
