using System;
using System.Collections.Generic;
using System.Linq;
using app.domain;
using Microsoft.EntityFrameworkCore;

namespace app.persistence
{
    public class ClientEntityFrameworkRepository : IClientRepository
    {
        private readonly DbContext _context;

        public ClientEntityFrameworkRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        /*public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }*/

        public void Delete(Object entity)
        {
            _context.Set<Object>().Remove(entity);
            _context.SaveChanges();
        }

        public void Add(Object entity)
        {
            _context.Set<Object>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(Object entity)
        {
            _context.Set<Object>().Update(entity);
            _context.SaveChanges();
        }

    }
}
