using MicroRabbit.Banking.Application.Models;
using MicroRabbitMq.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Interfaces;

public interface IAccountService
{
    IEnumerable<Account> GetAccounts();
    void Transfer(AccountTransfer accountTransfer);
    
}