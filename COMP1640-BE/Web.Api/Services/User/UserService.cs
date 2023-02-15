using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Web.Api.Data.Context;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using System.Linq;
    
namespace Web.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Entities.User> _userManager;
        private readonly IMapper _mapper;
        protected AppDbContext context;

        public UserService(UserManager<Entities.User> userManager, IMapper mapper, AppDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            this.context = context;
        }

        public async Task<IEnumerable<UserResponseModel>> GetAll()
        {
            try
            {
                //var users = await _userManager.Users.Include<Entities.Role>.;
                //var role = await _userManager.GetRolesAsync

                var usersWithRoles = (from user in context.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.UserName,
                                          Email = user.Email,
                                          RoleName = (from userRole in user.UserRoles
                                                       join role in context.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name)
                                      }).ToList().Select(p => new UserResponseModel()

                                      {
                                          Id = p.UserId,
                                          UserName = p.Username,
                                          Email = p.Email,
                                          Role = string.Join(",", p.RoleName)
                                      });
                return usersWithRoles;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.User> UpdateAsync(Guid id, Entities.User user)
        {
            try
            {
                if(_userManager.FindByIdAsync(id.ToString()) == null)
                {
                    throw new Exception("Can not find the user");
                }
                var update = _userManager.UpdateAsync(user); //error
                if (!update.IsCompletedSuccessfully)
                {
                    throw new Exception(update.Result.ToString());
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> Delete(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    throw new Exception("Can not find the user!");
                }
                var result = await _userManager.DeleteAsync(user);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
