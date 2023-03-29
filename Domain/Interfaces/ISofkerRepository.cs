using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISofkerRepository <TEntity> : IAdd<TEntity>, IEdit<TEntity>, IDelete, Domain.Interfaces.IList<TEntity>, ITransaction
    {
        void DeleteSofker(int entityTipeId, string entityId);
    }
}
