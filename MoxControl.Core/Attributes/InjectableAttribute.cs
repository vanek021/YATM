using Microsoft.Extensions.DependencyInjection;

namespace YATM.Core.Attributes
{
#pragma warning disable CS8618
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class InjectableAttribute : Attribute
    {
        public readonly Type ServiceType;
        public readonly ServiceLifetime? Lifetime;

        /// <summary>
        /// Inject this type as a service. For lifetime use global default value from `RegisterInjectableTypesFromAssemblies` call
        /// </summary>
        public InjectableAttribute()
        {
        }

        /// <summary>
        /// Inject this type as a service
        /// </summary>
        /// <param name="serviceType">Service type to implement</param>
        public InjectableAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }


        /// <summary>
        /// Inject this type as a service with lifetime
        /// </summary>
        /// <param name="contextLifetime">Set ServiceLifetime</param>
        public InjectableAttribute(ServiceLifetime contextLifetime)
        {
            Lifetime = contextLifetime;
        }

        /// <summary>
        /// Inject this type as a service. For lifetime use global default value from `RegisterInjectableTypesFromAssemblies` call
        /// </summary>
        /// <param name="serviceType">Service type to implement</param>
        /// <param name="contextLifetime">Set ServiceLifetime</param>
        public InjectableAttribute(Type serviceType, ServiceLifetime contextLifetime)
        {
            ServiceType = serviceType;
            Lifetime = contextLifetime;
        }
    }
#pragma warning restore CS8618
}
