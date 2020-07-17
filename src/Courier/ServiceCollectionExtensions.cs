using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Courier.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCourier(this IServiceCollection services, params Assembly[] assemblies)
        {
            
        }
    }
}