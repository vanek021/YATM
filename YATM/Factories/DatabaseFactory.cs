using Microsoft.AspNetCore.Components.Authorization;
using YATM.Core.Attributes;
using YATM.Core.Repositories;
using YATM.Data;

namespace YATM.Factories
{
    [Injectable(ServiceLifetime.Scoped)]
    [Injectable(typeof(IDatabaseFactory<Database>))]
    public class DatabaseFactory : BaseDatabaseFactory<Database>
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public DatabaseFactory(IServiceScopeFactory serviceScopeFactory, AuthenticationStateProvider authenticationStateProvider) : base(serviceScopeFactory)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public override Database Create()
        {
            return CreateWithScope(_authenticationStateProvider);
        }
    }

    [Injectable(ServiceLifetime.Singleton)]
    public class GlobalDatabaseFactory : BaseDatabaseFactory<Database>
    {
        public GlobalDatabaseFactory(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override Database Create()
        {
            return CreateWithScope();
        }
    }
}
