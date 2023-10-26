using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    if (configuration != null)
                    {
                        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                        var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                        if (rabbitMQSettings != null && serviceSettings != null)
                        {
                            configurator.Host(rabbitMQSettings.Host);
                            configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName));
                        }
                        else
                        {
                            throw new Exception($"Could not retrieve Configuration {nameof(ServiceSettings)} or {nameof(RabbitMQSettings)}");
                        }
                    }
                    else
                    {
                        throw new Exception("Could not retrieve object tipe IConfiguration");
                    }
                });
            });
            return services;
        }
    }
}