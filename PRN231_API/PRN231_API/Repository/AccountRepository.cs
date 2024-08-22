
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.ViewModel;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DAO;
using System;

namespace PRN231_API.Repository
{
    
    public class AccountRepository : IAccountRepository
    {
        private readonly SchoolDBContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(SchoolDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<CustomResponse> Register(Account userDto)
        {
            try
            {
                var user = _mapper.Map<Account>(userDto);
                user.ActiveCode = Guid.NewGuid().ToString();
                user.Type = "Student";
                user.IsActive = false;
                _context.Accounts.Add(user);
                _context.SaveChanges();
                return new CustomResponse { Message = "Success", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<CustomResponse> ActiveAccount(ActiveViewModel activeModel)
        {
            try
            {
                var user = _context.Accounts.SingleOrDefault(u => u.Email == activeModel.Email && u.ActiveCode == activeModel.ActivationCode);

                if (user == null)
                {
                    return new CustomResponse { Message = "Active Failed", StatusCode = 404 };
                }

                user.IsActive = true;
                user.ActiveCode = null;

                var newStudent = new Student
                {
                    IsRegularStudent = true,
                    AccountId = user.AccountId
                };

                _context.Students.Add(newStudent);
                _context.SaveChanges();

                var studentDetail = new StudentDetail
                {
                    StudentId = newStudent.StudentId 
                };

                _context.StudentDetails.Add(studentDetail);
                 _context.SaveChanges();
                return new CustomResponse { Message = "Success", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Account> Login(AccountLoginDto userDto)
        {
            try
            {
                return _context.Accounts.SingleOrDefault(u => u.Email == userDto.Email && u.Password == userDto.Password);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> GetUserByUsernameAsync(string username)
        {
            try
            {
                return _context.Accounts.SingleOrDefault(u => u.Email == username);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
