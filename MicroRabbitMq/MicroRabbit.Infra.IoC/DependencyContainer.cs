using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.infra.Bus;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        //Domain Bus
        services.TryAddTransient<IEventBus, RabbitMqBus>();

        // Domain Banking Commands
        services.TryAddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

        //Application Services
        services.TryAddTransient<IAccountService, AccountService>();
        services.TryAddTransient<ITransferService, TransferService>();

        // Data (use Scoped for DbContext)
        services.TryAddTransient<IAccountRepository, AccountRepository>();
        services.TryAddTransient<ITransferRepository, TransferRepository>();
        services.TryAddTransient<BankingDbContext>();
        services.TryAddTransient<TransferDbContext>();
    }
}