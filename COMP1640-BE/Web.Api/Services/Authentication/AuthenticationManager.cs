using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using Web.Api.DTOs.RequestModels;
using Web.Api.Configuration;
using Microsoft.Extensions.Options;

namespace Web.Api.Services.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<Entities.User> _userManager;
        private readonly JwtConfig _jwtConfig;
        private Entities.User _user;
        public AuthenticationManager(UserManager<Entities.User> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        public async Task<bool> ValidateUser(UserForAuthenRequestModel userForAuth)
        {
            try
            {
                _user = await _userManager.FindByEmailAsync(userForAuth.Email);
                return (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires:DateTime.Now.AddHours(Convert.ToDouble(_jwtConfig.Expires)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}
