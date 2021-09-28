using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BudgetUnderControl.Common.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BudgetUnderControl.MobileDomain
{
    public class ContextFacade : IContextFacade
    {
        private DbConnection _connection;

        public int SaveChanges() => _context.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => _context.SaveChangesAsync(cancellationToken);

        public EntityEntry Remove(object entity) => _context.Remove(entity);

        public EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class => _context.Remove<TEntity>(entity);

        public void RemoveRange(IEnumerable<object> entities) => _context.Remove(entities);

        private readonly Context _context;

        public static ContextFacade Create(IContextConfig contextConfig) => new ContextFacade(contextConfig);

        public ContextFacade(IContextConfig contextConfig)
        {
            if(contextConfig.Application == ApplicationType.Test)
            {
                if(_connection == null)
                {
                    _connection = new SqliteConnection("DataSource=:memory:");
                    _connection.Open();
                    var optionsBuilder = new DbContextOptionsBuilder<Context>()
                    .UseSqlite(_connection, options => options.MigrationsAssembly("BudgetUnderControl.Migrations.SQLite")).Options;
                    _context = Context.CreateTest(optionsBuilder, contextConfig);
                }
            }
            else
            {
                _context = Context.Create(contextConfig);
            }
        }

        public void SetEntityState(object entity, EntityState state)
        {
            this._context.Entry(entity).State = state;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public DbSet<Account> Accounts
        {
            get => _context.Accounts;
            set => _context.Accounts = value;
        }

        public DbSet<AccountSnapshot> AccountSnapshot
        {
            get => _context.AccountSnapshot;
            set => _context.AccountSnapshot = value;
        }
        public DbSet<Category> Categories
        {
            get => _context.Categories;
            set => _context.Categories = value;
        }
        public DbSet<Currency> Currencies
        {
            get => _context.Currencies;
            set => _context.Currencies = value;
        }
        public DbSet<ExchangeRate> ExchangeRates
        {
            get => _context.ExchangeRates;
            set => _context.ExchangeRates = value;
        }
        public DbSet<File> Files
        {
            get => _context.Files;
            set => _context.Files = value;
        }
        public DbSet<FileToTransaction> FilesToTransactions
        {
            get => _context.FilesToTransactions;
            set => _context.FilesToTransactions = value;
        }
        public DbSet<Icon> Icons
        {
            get => _context.Icons;
            set => _context.Icons = value;
        }
        public DbSet<Tag> Tags
        {
            get => _context.Tags;
            set => _context.Tags = value;
        }
        public DbSet<TagToTransaction> TagsToTransactions
        {
            get => _context.TagsToTransactions;
            set => _context.TagsToTransactions = value;
        }
        public DbSet<Transaction> Transactions
        {
            get => _context.Transactions;
            set => _context.Transactions = value;
        }
        public DbSet<Transfer> Transfers
        {
            get => _context.Transfers;
            set => _context.Transfers = value;
        }
        public DbSet<User> Users
        {
            get => _context.Users;
            set => _context.Users = value; 
        }
        public DbSet<Synchronization> Synchronizations
        {
            get => _context.Synchronizations;
            set => _context.Synchronizations = value;
        }
    }
}
