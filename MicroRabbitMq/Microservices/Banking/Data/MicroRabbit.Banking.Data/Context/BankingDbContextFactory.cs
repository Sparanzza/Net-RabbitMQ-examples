using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MicroRabbit.Banking.Data.Context;

// https://www.daveabrock.com/2021/01/19/config-top-level-programs/
public class BankingDbContextFactory : IDesignTimeDbContextFactory<BankingDbContext>
{
    public BankingDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BankingDbContext>();
        var connectionString = configuration.GetConnectionString("BankingDbConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new BankingDbContext(optionsBuilder.Options);
    }
}