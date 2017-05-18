using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.domain
{
    public interface IClientRepository<T> where T : Entity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);

        void Delete(T entity);
        void Add(T entity);
        void Update(T entity);
    }
}
