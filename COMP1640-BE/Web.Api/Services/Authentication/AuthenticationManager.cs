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

        private string logoUrl = "https://res.cloudinary.com/duasvwfje/image/upload/v1678291119/GoldenIdeaImg/GoldenIdea_prupeg.png";

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
            catch (Exception)
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
            string html = "<table width=\"100%\" bgcolor=\"#f2f3f8\"\r\n  style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\r\n  <tr>\r\n  <tr>\r\n      <td style=\"height:50px;\">&nbsp;</td>\r\n  </tr>\r\n    <td>\r\n      <table style=\"background-color: #f2f3f8; max-width:670px;  margin:auto auto;\" width=\"100%\" align=\"center\">\r\n        <tr>\r\n          <td>\r\n            <table width=\"95%\" border=\"0\" align=\"center\"\r\n              style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;\">\r\n              <tr>\r\n                <td style=\"text-align:center;\">\r\n                  <a title=\"logo\">\r\n                    <img width=\"25%\"\r\n                      src=\"" +
                logoUrl + "\"\r\n                      title=\"logo\" alt=\"logo\">\r\n                  </a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"padding:0 35px;\">\r\n                  <h1 style=\"color:#1e1e2d; font-weight:500;font-size:32px;font-family:'Rubik',sans-serif;\">\r\n                    You have\r\n                    requested to reset your password</h1>\r\n                  <span\r\n                    style=\"display:inline-block; vertical-align:middle; margin:10px 0 10px; border-bottom:1px solid #cecece; width:100px;\">";
            string html1 = "</span>\r\n                  <p style=\"color:#455056; font-size:1em;line-height:24px;\">\r\n                    We cannot simply send you your old password. A unique link to reset your\r\n                    password has been generated for you. To reset your password, click the\r\n                    following link and follow the instructions.\r\n                  </p>\r\n                  <a href=\"";
            string html2 = "\"\r\n                    style=\"background:#f6f872;text-decoration:none !important; font-weight:500; margin-top:35px; color:#000000;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">Reset\r\n                    Password</a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"height:40px;\">&nbsp;</td>\r\n              </tr>\r\n            </table>\r\n          </td>\r\n        <tr>\r\n          <td style=\"height:20px;\">&nbsp;</td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"text-align:center;\">\r\n            <p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\r\n              &copy; <strong>www.GoldenIdea.com</strong></p>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"height:50px;\">&nbsp;</td>\r\n        </tr>\r\n      </table>\r\n    </td>\r\n  </tr>\r\n</table>";
            string mainHtml = html1 + appDomain + confirmLink + html2;
            SendEmailOptions option = new SendEmailOptions
            {
                ToName = user.Name,
                ToEmail = user.Email,
                Body = string.Format(html + "<b>" + user.UserName + "</b>" + mainHtml, resetPasswordId),
                Subject = "[Golden Idea] Change password"
            };
            var result = await _emailService.SendEmailAsync(option.ToName, option.ToEmail, option.Subject, option.Body);
            return result;
        }
        public async Task<bool> SendEmailRegister(UserRequestModel user)
        {
            string html = "<table width=\"100%\" bgcolor=\"#f2f3f8\"\r\n  style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\r\n  <tr>\r\n  <tr>\r\n      <td style=\"height:50px;\">&nbsp;</td>\r\n  </tr>\r\n    <td>\r\n      <table style=\"background-color: #f2f3f8; max-width:670px;  margin:auto auto;\" width=\"100%\" align=\"center\">\r\n        <tr>\r\n          <td>\r\n            <table width=\"95%\" border=\"0\" align=\"center\"\r\n              style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;\">\r\n              <tr>\r\n                <td style=\"text-align:center;\">\r\n                  <a title=\"logo\">\r\n                    <img width=\"25%\"\r\n                      src=\"" +
                logoUrl + "\"\r\n                      title=\"logo\" alt=\"logo\">\r\n                  </a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"padding:0 35px;\">\r\n                  <h1 style=\"color:#1e1e2d; font-weight:500;font-size:32px;font-family:'Rubik',sans-serif;\">\r\n                    Your account\r\n                   is registered successful!</h1>\r\n                  <span\r\n                    style=\"display:inline-block; vertical-align:middle; margin:10px 0 10px; border-bottom:1px solid #cecece; width:100px;\">";
            string html1 = "</span>\r\n                  <p style=\"color:#455056; font-size:1em;line-height:24px;\">\r\n                    Please use this password to login to Golden Ideas and remember to change your password!\r\n                   </p>\r\n                  <a href=\"";
            string html2 = "\"\r\n                    style=\"background:#f6f872;text-decoration:none !important; font-weight:500; margin-top:35px; color:#000000;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">Thank\r\n                    You</a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"height:40px;\">&nbsp;</td>\r\n              </tr>\r\n            </table>\r\n          </td>\r\n        <tr>\r\n          <td style=\"height:20px;\">&nbsp;</td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"text-align:center;\">\r\n            <p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\r\n              &copy; <strong>www.GoldenIdea.com</strong></p>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"height:50px;\">&nbsp;</td>\r\n        </tr>\r\n      </table>\r\n    </td>\r\n  </tr>\r\n</table>";
            string mainHtml = html1 + html2;
            SendEmailOptions option = new SendEmailOptions
            {
                ToName = user.Name,
                ToEmail = user.Email,
                Body = string.Format(html + "<b>" + user.Password + "</b>" + mainHtml),
                Subject = "[Golden Idea] Sign Up Success!"
            };
            var result = await _emailService.SendEmailAsync(option.ToName, option.ToEmail, option.Subject, option.Body);
            return result;
        }
    }
}

