using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace my_portfolio_mvc.Infrastructure.DataProtection;

public sealed class DataProtectionKeyContext
    : DbContext, IDataProtectionKeyContext
{
    public DataProtectionKeyContext(
        DbContextOptions<DataProtectionKeyContext> options)
        : base(options)
    {
    }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        = null!;
}