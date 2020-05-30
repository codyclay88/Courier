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
                // Register Non-Generic Receiver Types
                assembly.GetTypes()
                    .Where(t => t.BaseType != null
                             && t.BaseType == typeof(HostedCourierReceiverBase))
                    .ToList()
                    .ForEach((type) => services.TryAddEnumerable(
                            ServiceDescriptor.Singleton(typeof(IHostedService), type)));

                // Register Generic Receiver Types
                assembly.GetTypes()
                    .Where(t => typeof(IMessage).IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract)
                    .Select(messageType => typeof(HostedCourierReceiverBase<>).MakeGenericType(messageType))
                    .SelectMany(baseProcessorType => assembly.GetTypes().Where(
                                t => t.BaseType != null
                                 && t.BaseType.IsGenericType
                                 && t.BaseType == baseProcessorType))
                    .ToList()
                    .ForEach(processorType => services.TryAddEnumerable(
                            ServiceDescriptor.Singleton(typeof(IHostedService), processorType)));
            }
        }
    }
}