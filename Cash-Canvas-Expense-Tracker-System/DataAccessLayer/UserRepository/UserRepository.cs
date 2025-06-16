using CashCanvas.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CashCanvas.Data.UserRepository;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<UserDTO?> GetActiveUserByRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Where(t => t.Token == token && t.IsActive && !t.IsRevoked)
            .Include(t => t.User)
            .AsNoTracking()
            .Select(t => new UserDTO
            {
                UserId = t.UserId,
                UserName = t.User.UserName,
                UserEmail = t.User.Email,
                CreatedAt = t.User.CreatedAt
            })
            .FirstOrDefaultAsync();
    }
}
