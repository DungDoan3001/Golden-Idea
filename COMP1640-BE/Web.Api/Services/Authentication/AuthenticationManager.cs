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
using Web.Api.Services.EmailService;
using Web.Api.Services.ResetPassword;

namespace Web.Api.Services.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<Entities.User> _userManager;
        private readonly JwtConfig _jwtConfig;
        private Entities.User _user;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IResetPasswordService _resetPasswordService;
        public AuthenticationManager(UserManager<Entities.User> userManager, IOptionsMonitor<JwtConfig> optionsMonitor, IConfiguration configuration, IEmailService emailService, IResetPasswordService resetPasswordService)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _configuration = configuration;
            _emailService = emailService;
            _resetPasswordService = resetPasswordService;
        }
        public async Task<bool> ValidateUser(UserForAuthenRequestModel userForAuth)
        {
            try
            {
                _user = await _userManager.FindByEmailAsync(userForAuth.Email);
                return (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
            }
            catch (Exception ex)
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
            if(_user.Avatar != null)
            {
                claims.Add(new Claim("Avatar", _user.Avatar));
            }
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
                expires: DateTime.Now.AddHours(Convert.ToDouble(_jwtConfig.Expires)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        public async Task<bool> GenerateChangePasswordTokenAsync(Entities.User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                
                var checkResetPassword = await _resetPasswordService.FindByUserIdAsync(user.Id);
                if (checkResetPassword != null)
                {
                    foreach (var userReset in checkResetPassword)
                    {
                        await _resetPasswordService.DeleteAsync(userReset.Id);
                    }
                }
                Entities.ResetPassword resetPassword = new Entities.ResetPassword()
                {
                    UserId = user.Id,
                    Token = token
                };
                await _resetPasswordService.CreateAsync(resetPassword);
                var userResetPassword = await _resetPasswordService.FindByUserIdAsync(user.Id);
                foreach (var item in userResetPassword)
                {
                    await SendEmailChangePassword(user, Helpers.GuidBase64.Encode(item.Id));
                }
                return true;
            }
            return false;
        }

        public async Task<bool> SendEmailChangePassword(Entities.User user, string resetPasswordId)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmLink = _configuration.GetSection("Application:ChangePassword").Value;
            SendEmailOptions option = new SendEmailOptions
            {
                ToName = user.Name,
                ToEmail = user.Email,
                Body = string.Format("Here is your link to change your password for your account (this link will be expired in 24 hours): <a href=\"" 
                    + appDomain + confirmLink + "\">Click Here</a>", resetPasswordId),
                Subject = "[Golden Idea] Change password"
            };
            var result = await _emailService.SendEmailAsync(option.ToName, option.ToEmail, option.Subject, option.Body);
            return result;
        }
    }
}

