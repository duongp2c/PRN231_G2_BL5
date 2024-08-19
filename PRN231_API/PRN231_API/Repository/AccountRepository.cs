
using Microsoft.EntityFrameworkCore;
using PRN231_API.DAO;
using PRN231_API.Models;
using System;

namespace PRN231_API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SchoolDBContext _context;

        public AccountRepository(SchoolDBContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<bool> SaveResetTokenAsync(int accountId, string token)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.ActiveCode = token;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Account?> GetAccountByResetTokenAsync(string token)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.ActiveCode == token);
        }

        public async Task<bool> UpdatePasswordAsync(int accountId, string newPassword)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.Password = newPassword;  // Ensure this is hashed!
            account.ActiveCode = null;  // Invalidate the token
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
