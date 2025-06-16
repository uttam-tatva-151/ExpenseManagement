using CashCanvas.Core.DTOs;

namespace CashCanvas.Data.UserRepository;

public interface IUserRepository
{
    Task<UserDTO?> GetActiveUserByRefreshTokenAsync(string token);
}
