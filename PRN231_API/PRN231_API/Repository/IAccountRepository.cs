using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.ViewModel;

namespace PRN231_API.Repository
{
    public interface IAccountRepository
    {
        Task<CustomResponse> Register(Account user);
        Task<Account> Login(AccountLoginDto user);
        Task<Account> GetUserByUsernameAsync(string username);
        Task<CustomResponse> ActiveAccount(ActiveViewModel activeModel);
        Task<Account?> GetAccountByEmailAsync(string email);
            Task<bool> SaveResetTokenAsync(int accountId, string token);
            Task<Account?> GetAccountByResetTokenAsync(string token);
            Task<bool> UpdatePasswordAsync(int accountId, string newPassword);
        

    }
}
