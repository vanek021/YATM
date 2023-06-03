using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATM.Core.Interfaces;
using IDKEY = System.Int64;

namespace YATM.Core.Repositories
{
    public interface IReadableRepository<T>
        where T : class, IEntity
    {
        T GetById(IDKEY id);
        IEnumerable<T> GetManyByIds(IEnumerable<IDKEY> idKeysList);
        bool Contains(IDKEY id);

        #region Async

        Task<T> GetByIdAsync(IDKEY id);
        IAsyncEnumerable<T> GetManyByIdsAsync(IEnumerable<IDKEY> idKeysList);
        Task<bool> ContainsAsync(IDKEY id);

        #endregion
    }

    public interface IDatabase
    {
        void SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public interface IScopeManager
    {
        public object GetService(Type type);
        public object CreateInstance(Type instanceType, params object[] parameters);
    }
}
