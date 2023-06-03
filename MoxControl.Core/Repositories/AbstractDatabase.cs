using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YATM.Core.Interfaces;
using YATM.Core.Internal;
using YATM.Core.Models;

namespace YATM.Core.Repositories
{
    public abstract class AbstractDatabase : IDatabase, IDisposable, IDatabaseServiceScopeOwnerConfiguration, IScopeManager
    {
        protected DbContext Context { get; private set; }
        public bool IsDisposed { get; private set; } = false;
        protected IServiceScope ServiceScope { get; private set; }

        private HashSet<Type> _blackListTypes = new HashSet<Type>();
        public AbstractDatabase(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            Context = context;
#if DEBUG
            UniqueDbContextId = System.Threading.Interlocked.Increment(ref TotalContexts);
#endif
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public virtual T GetService<T>()
        {
            if (ServiceScope == null)
                throw new InvalidOperationException("GetService() method should be used only with Database created from DatabaseFactory");

            if (_blackListTypes.Contains(typeof(T)))
                throw new InvalidOperationException("service denied");

            return ServiceScope.ServiceProvider.GetService<T>();
            //    var result = ActivatorUtilities.CreateInstance<T>(ServiceProvider, this);
            //    return result;
        }

        public virtual object GetService(Type type)
        {
            if (ServiceScope == null)
                throw new InvalidOperationException("GetService() method should be used only with Database created from DatabaseFactory");

            if (_blackListTypes.Contains(type))
                throw new InvalidOperationException("service denied");

            return ServiceScope.ServiceProvider.GetRequiredService(type);
            //    var result = ActivatorUtilities.CreateInstance<T>(ServiceProvider, this);
            //    return result;
        }

        public virtual object CreateInstance(Type instanceType, params object[] parameters)
        {
            if (ServiceScope == null)
                throw new InvalidOperationException("GetService() method should be used only with Database created from DatabaseFactory");

            if (_blackListTypes.Contains(instanceType))
                throw new InvalidOperationException("service denied");

            return ActivatorUtilities.CreateInstance(ServiceScope.ServiceProvider, instanceType, parameters);
        }

        public void Dispose()
        {
            // If Database class is context and services owner
            if (ServiceScope != null && !IsDisposed)
            {
                lock (this)
                {
                    if (ServiceScope == null || IsDisposed)
                        return;

                    try
                    {
                        IsDisposed = true;
                        //if (ownContext != null)
                        //    ownContext.Dispose();
                        ServiceScope.Dispose();
                    }
                    finally
                    {
                        ServiceScope = null;
                        Context = null;
                        IsDisposed = true;
                    }
                }
            }
        }

        void IDatabaseServiceScopeOwnerConfiguration.SetOwningServiceScope(IServiceScope scope/*, DbContext context*/)
        {
            ServiceScope = scope;
            //ownContext = context;
        }

        void IDatabaseServiceScopeOwnerConfiguration.AddBlackServiceType(Type type)
        {
            _blackListTypes.Add(type);
        }

#if DEBUG
        public long UniqueDbContextId;
        public static long TotalContexts = 0;
#endif
    }

    public interface IDatabaseFactory<TDatabase>
        where TDatabase : IDatabase
    {
        TDatabase Create();
    }

    public abstract class BaseDatabaseFactory<TDatabase> : IDatabaseFactory<TDatabase>
        where TDatabase : class, IDatabase
    {
        IServiceScopeFactory factory;

        public BaseDatabaseFactory(IServiceScopeFactory serviceScopeFactory)
        {
            factory = serviceScopeFactory;
        }

        public abstract TDatabase Create();

        // Do not use. Сделано для размышлений о вечном...
        protected TDatabase CronstructFromContextFactoryNotRecommended()
        {
            var scope = factory.CreateScope();

            //var constructed = scope.ServiceProvider.GetService<Ability.Core.Internal.IIdentityDbContexFactory>().CreateDbContext();
            var constructed = scope.ServiceProvider.GetService<IDbContextFactory<DbContext>>().CreateDbContext();
            if (constructed != null)
            {
                var tp = typeof(TDatabase);
                var ctor = tp.GetConstructors().First();
                var data = new List<object>();
                data.Add(constructed);
                foreach (var it in ctor.GetParameters().Skip(1))
                {
                    var atp = it.ParameterType;
                    var arg = Activator.CreateInstance(atp, constructed);
                    data.Add(arg);
                }
                var database = Activator.CreateInstance(tp, data.ToArray()) as TDatabase;

                var configurator = database as YATM.Core.Internal.IDatabaseServiceScopeOwnerConfiguration;
                configurator.SetOwningServiceScope(scope);

                return database;
            }
            throw new InvalidOperationException("No context factory. Please call CreateFromScope() instead");
        }

        protected TDatabase CreateWithScope(AuthenticationStateProvider authenticationStateProvider = null)
        {
            var scope = factory.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<TDatabase>();
            var configurator = database as YATM.Core.Internal.IDatabaseServiceScopeOwnerConfiguration;

            configurator.AddBlackServiceType(typeof(IHttpContextAccessor));
            if (authenticationStateProvider != null)
            {
                (scope.ServiceProvider.GetService<IAuthenticationStateAccessor>() as AuthenticationStateAccessor).SetProvider(authenticationStateProvider);
            }
            else
            {
                configurator.AddBlackServiceType(typeof(IAuthenticationStateAccessor));
            }

            configurator.SetOwningServiceScope(scope);

            return database;
        }
    }
}
