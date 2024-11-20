using Dashboard.DAL;
using Dashboard.DAL.Models;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard.BLL.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public JwtService(IConfiguration configuration, AppDbContext context, IUserRepository userRepository)
        {
            _configuration = configuration;
            _context = context;
            _userRepository = userRepository;
        }

        private async Task SaveRefreshTokenAsync(User user, string refreshToken, string jwtId)
        {
            var token = new RefreshToken
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddDays(7),
                isUsed = false,
                JwtId = jwtId,
                Token = refreshToken,
                UserId = user.Id
            };

            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        private JwtSecurityToken GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email ?? "no email"),
                new Claim("firstName", user.FirstName ?? "no first name"),
                new Claim("lastName", user.LastName ?? "no last name")
            };

            var roleClaims = user.UserRoles.Select(ur => new Claim("role", ur.Role.Name));

            if (roleClaims.Count() > 0)
            {
                claims.AddRange(roleClaims);
            }
            else
            {
                claims.Add(new Claim("role", Settings.UserRole));
            }

            var issuer = _configuration["AuthSettings:issuer"];
            var audience = _configuration["AuthSettings:audience"];
            var keyString = _configuration["AuthSettings:key"];
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[32];

            using(var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<ServiceResponse> GenerateTokensAsync(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(user, refreshToken, accessToken.Id);

            var tokens = new JwtVM
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };

            return ServiceResponse.GetOkResponse("Jwt токени", tokens);
        }

        public async Task<ServiceResponse> RefreshTokensAsync(string refreshToken, string accessToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if(storedToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            if(storedToken.isUsed)
            {
                throw new SecurityTokenException("Invalid token");
            }
            
            if (storedToken.ExpireDate < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Token expired");
            }

            var principal = GetPrincipals(accessToken);

            var accessTokenId = principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            if(storedToken.JwtId != accessTokenId)
            {
                throw new SecurityTokenException("Invalid access token");
            }

            storedToken.isUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();


            var user = await _userRepository.GetUserByIdAsync(storedToken.UserId.ToString(), true);

            if(user == null)
            {
                return ServiceResponse.GetBadRequestResponse($"User by id {storedToken.UserId} not found", errors: $"Користувача з id {storedToken.UserId} не знайдено");
            }

            var response = await GenerateTokensAsync(user);

            return response;
        }

        private ClaimsPrincipal GetPrincipals(string accessToken)
        {
            var jwtSecurityKey = _configuration["AuthSettings:key"];

            var validationParamteres = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey))
            };

            var tokenHanlder = new JwtSecurityTokenHandler();
            var principal = tokenHanlder.ValidateToken(accessToken, validationParamteres, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if(jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new SecurityTokenException("Invalid access token");
            }

            return principal;
        }
    }
}
