using CetproNicol.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CetproNicol.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CetproNicolDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(CetproNicolDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_transaction is not null)
                await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
