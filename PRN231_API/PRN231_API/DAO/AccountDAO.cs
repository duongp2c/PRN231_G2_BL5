using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;
using PRN231_API.ViewModel;
using static PRN231_API.DAO.AccountDAO;

namespace PRN231_API.DAO
{
    public interface IAccountService
    {
        Task<CustomResponse> RegisterAccount(AccountRegisterDto userDto);
        Task<TokenModel> LoginAccount(AccountLoginDto userDto);
        Task<TokenModel> Refresh(TokenModel tokenModel);
        Task<CustomResponse> ActiveAccount(ActiveViewModel activeModel);
    }
    public class AccountDAO : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly EmailDAO _emailDAO;

        public AccountDAO(IAccountRepository accountRepository, IMapper mapper, IJwtTokenService tokenService, EmailDAO emailDAO)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _jwtTokenService = tokenService;
            _emailDAO = emailDAO;
        }

        public class ActivateViewModel
        {
            public string Email { get; set; }
            public string ActivationCode { get; set; }
        }

        public async Task<TokenModel> LoginAccount(AccountLoginDto userDto)
        {
            try
            {
                var user = await _accountRepository.Login(userDto);
                if (user == null)
                    return null;

                var token = _jwtTokenService.GenerateToken(user);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                await _jwtTokenService.SaveRefreshTokenToRedisAsync(refreshToken, user.Email);


                return new TokenModel { AccessToken = token, RefreshToken = refreshToken };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TokenModel> Refresh([FromBody] TokenModel model)
        {
            var refreshtoken = await _jwtTokenService.GetRefreshTokenFromRedisAsync(model.Email);
            if (refreshtoken == null)
            {
                return null;
            }

            var user = await _accountRepository.GetUserByUsernameAsync(model.Email);
            if (user == null)
            {
                return null;
            }

            var newAccessToken = _jwtTokenService.GenerateToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _jwtTokenService.SaveRefreshTokenToRedisAsync(newRefreshToken, user.Email);

            return new TokenModel { AccessToken = newAccessToken, RefreshToken = newRefreshToken };

        }

        public async Task<CustomResponse> RegisterAccount(AccountRegisterDto userDto)
        {
            try
            {
                var user = _mapper.Map<Account>(userDto);
                var response = await _accountRepository.Register(user);
                var subject = "Your Activation Code";
                var body = $"Your activation code is {user.ActiveCode}. Please use this code to activate your account.";
                _emailDAO.SendEmail(user.Email, subject, body);

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
                var user = await _accountRepository.ActiveAccount(activeModel);
                return new CustomResponse { Message = "Success", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
