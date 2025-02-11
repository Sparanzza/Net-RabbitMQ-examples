using MicroRabbitMq.Banking.Data.Context;
using MicroRabbitMq.Banking.Domain.Interfaces;
using MicroRabbitMq.Banking.Domain.Models;

namespace MicroRabbitMq.Banking.Data.Repository;

public class AccountRepository: IAccountRepository
{
    private readonly BankingDbContext _ctx;

    public AccountRepository(BankingDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public IEnumerable<Account> GetAccounts()
    {
        return _ctx.Accounts;
    }
}