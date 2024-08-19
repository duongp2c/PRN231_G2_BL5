using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface IAccountRepository
    {
       
            Task<Account?> GetAccountByEmailAsync(string email);
            Task<bool> SaveResetTokenAsync(int accountId, string token);
            Task<Account?> GetAccountByResetTokenAsync(string token);
            Task<bool> UpdatePasswordAsync(int accountId, string newPassword);
        

    }
}
