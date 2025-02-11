using MicroRabbitMq.Banking.Domain.Models;

namespace MicroRabbitMq.Banking.Domain.Interfaces;

public interface IAccountRepository
{
    IEnumerable<Account> GetAccounts();
    
}