using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using PRN231_API.Models;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_API.DAO
{
    public interface IJwtTokenService
    {
        string GenerateToken(Account user);
        string GenerateRefreshToken();
        Task<string> GetRefreshTokenFromRedisAsync(string refreshToken);
        Task SaveRefreshTokenToRedisAsync(string refreshToken, string username);
    }
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _configuration;
        private readonly string _key;

        public JwtTokenService(IConfiguration configuration, IConnectionMultiplexer cache)
        {
            _configuration = configuration;
            _redis = cache;
        }

        public string GenerateToken(Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("UserID", user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Type)
                }),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<string> GetRefreshTokenFromRedisAsync(string username)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync($"refresh_token:{username}");
        }

        public async Task SaveRefreshTokenToRedisAsync(string refreshToken, string username)
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.StringSetAsync($"refresh_token:{username}", refreshToken, TimeSpan.FromDays(30));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
