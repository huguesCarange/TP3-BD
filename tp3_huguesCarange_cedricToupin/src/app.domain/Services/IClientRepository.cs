using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.domain
{
    public interface IClientRepository
    {
        void Delete(Object obj);
        void Add(Object obj);
        void Update(Object obj);
    }
}
