using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Courier.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCourier(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<ICourier, Courier>();

            foreach (var assembly in assemblies)
            {
                Console.WriteLine("Finding message types for: {0}", assembly.GetName().Name);

                var messageTypes = assembly
                .GetTypes()
                .Where(t => typeof(IMessage).IsAssignableFrom(t)
                    && !t.IsInterface
                    && !t.IsAbstract);

                foreach (var messageType in messageTypes)
                {
                    var baseProcessorType = typeof(HostedCourierReceiverBase<>)
                        .MakeGenericType(messageType);

                    Console.WriteLine("Finding processors for: {0}", baseProcessorType.Name);

                    var processorTypes = assembly
                        .GetTypes()
                        .Where(t => t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType == baseProcessorType);

                    foreach (var procType in processorTypes)
                    {
                        Console.WriteLine("Registering: {0}", procType.Name);

                        services.TryAddEnumerable(
                            ServiceDescriptor.Singleton(typeof(IHostedService), procType));
                    }
                }
            }
        }
    }
}