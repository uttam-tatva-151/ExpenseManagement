using CashCanvas.Core.Entities;
using CashCanvas.Data.BaseRepository;
using CashCanvas.Data.BillRepository;
using CashCanvas.Data.UserRepository;

namespace CashCanvas.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IUserRepository CustomUserRepository { get; }
        IGenericRepository<PasswordRecoveryToken> PasswordRecoveryTokens { get; }
        IGenericRepository<RefreshToken> RefreshTokens { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Transaction> Transactions { get; }
        IGenericRepository<Bill> Bills { get; }
        IBillRepository CustomBillRepository { get; }
        IGenericRepository<Reminder> Reminders { get; }
        IGenericRepository<Budget> Budgets { get; }
        IGenericRepository<Payment> Payments { get; }

        Task<int> CompleteAsync();
    }