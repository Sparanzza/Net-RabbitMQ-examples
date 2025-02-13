using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.infra.Bus;
using MicroRabbitMq.Banking.Data.Context;
using MicroRabbitMq.Banking.Data.Repository;
using MicroRabbitMq.Banking.Domain.CommandHandlers;
using MicroRabbitMq.Banking.Domain.Commands;
using MicroRabbitMq.Banking.Domain.Events;
using MicroRabbitMq.Banking.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        //Domain Bus
        services.AddTransient<IEventBus, RabbitMqBus>();
        
        // Domain Banking Commands
        services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler >();
        
        //Application Services
        services.AddTransient<IAccountService, AccountService>();
        
        //Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<BankingDbContext>();
    }
}