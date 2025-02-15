using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEventBus _bus;

    public AccountService(IAccountRepository accountRepository, IEventBus eventBus)
    {
        _accountRepository = accountRepository;
        _bus = eventBus;
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _accountRepository.GetAccounts();
    }

    public void Transfer(AccountTransfer accountTransfer)
    {
        var createTransferCommand = new CreateTransferCommand(accountTransfer.FromAccount, accountTransfer.ToAccount,
            accountTransfer.TransferAmount);
        _bus.SendCommand(createTransferCommand);
    }
}