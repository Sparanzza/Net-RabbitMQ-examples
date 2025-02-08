using System.Reflection;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.infra.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IEventBus, RabbitMqBus>();
    }
}