using CashCanvas.Core.Entities;
using CashCanvas.Data.BaseRepository;
using CashCanvas.Data.BillRepository;
using CashCanvas.Data.UserRepository;

namespace CashCanvas.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<User> Users { get; }
        public IGenericRepository<PasswordRecoveryToken> PasswordRecoveryTokens { get; }
        public IGenericRepository<RefreshToken> RefreshTokens { get; }
        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<Transaction> Transactions { get; }
        public IGenericRepository<Bill> Bills { get; }
        public IGenericRepository<Reminder> Reminders { get; }
        public IGenericRepository<Budget> Budgets { get; }
        public IGenericRepository<Payment> Payments { get; }
        public IGenericRepository<Notifications> Notifications { get; }
        public IUserRepository CustomUserRepository { get; }
        public IBillRepository CustomBillRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CustomUserRepository = new Data.UserRepository.UserRepository(_context);
            Users = new GenericRepository<User>(_context);
            PasswordRecoveryTokens = new GenericRepository<PasswordRecoveryToken>(_context);
            RefreshTokens = new GenericRepository<RefreshToken>(_context);
            Categories = new GenericRepository<Category>(_context);
            Transactions = new GenericRepository<Transaction>(_context);
            Bills = new GenericRepository<Bill>(_context);
            CustomBillRepository = new Data.BillRepository.BillRepository(_context);
            Reminders = new GenericRepository<Reminder>(_context);
            Budgets = new GenericRepository<Budget>(_context);
            Payments = new GenericRepository<Payment>(_context);
            Notifications = new GenericRepository<Notifications>(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}