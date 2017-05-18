using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.domain.Entities;

namespace app.domain
{
    public interface IArtisteRepository<T> where T : Entity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);

        void Delete(T entity);
        void Add(T entity);
        void Update(T entity);

    }
}
