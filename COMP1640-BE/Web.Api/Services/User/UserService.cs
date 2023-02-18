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
using Web.Api.DTOs.RequestModels;
namespace Web.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Entities.User> _userManager;
        protected AppDbContext context;
        private IPasswordHasher<Entities.User> _passwordHasher;

        public UserService(UserManager<Entities.User> userManager, AppDbContext context, IPasswordHasher<Entities.User> passwordHasher)
        {
            _userManager = userManager;
            this.context = context;
            this._passwordHasher = passwordHasher;
        }

        public async Task<List<Entities.User>> GetAll()
        {
            try
            {
                var users = _userManager.Users.ToList();
                //https://www.youtube.com/watch?v=6JVZwwAf88k
                //var usersWithRoles = await context.Users 
                //    .Include(x => x.UserRoles)
                //    .ThenInclude(x => x.Role)
                //    .ToListAsync();
                
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.User> GetById(Guid id)
        {
            try
            {
                var user = _userManager.FindByIdAsync(id.ToString());
                return user.Result;
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public async Task<Entities.User> UpdateAsync(Guid id, UserRequestModel user)
        {
            try
            {
                var userUpdate = _userManager.FindByIdAsync(id.ToString());
                if (userUpdate == null)
                {
                    throw new Exception("Can not find the user");
                }    
                userUpdate.Result.UserName = user.UserName;
                userUpdate.Result.Email = user.Email;
                userUpdate.Result.Name = user.Name;
                userUpdate.Result.PasswordHash = _passwordHasher.HashPassword(userUpdate.Result, user.Password);
                userUpdate.Result.Address = user.Address;
                userUpdate.Result.DepartmentId = user.DepartmentId;
                userUpdate.Result.PhoneNumber = user.PhoneNumber;
                //Check email and username is existed
                var users = _userManager.Users.ToList();
                foreach(var item in users)
                {
                    if(item.Email.ToLower().Trim() == userUpdate.Result.Email.ToLower().Trim())
                    {
                        throw new Exception("The email has been used, please choose another email!");
                    }
                    if(item.UserName.ToLower().Trim() == userUpdate.Result.UserName.ToLower().Trim())
                    {
                        throw new Exception("The username has been used, please choose another username!");
                    }
                }
                var update = await _userManager.UpdateAsync(userUpdate.Result);
                //Add role
                var userRole = _userManager.GetRolesAsync(userUpdate.Result);
                if (userRole.ToString().Trim() != user.Role.Trim().ToLower())
                {
                    await _userManager.RemoveFromRoleAsync(userUpdate.Result, userRole.Result[0]);
                }
                if (userRole == null)
                {
                    await _userManager.RemoveFromRoleAsync(userUpdate.Result, null);
                }
                await _userManager.AddToRoleAsync(userUpdate.Result, user.Role);
                if (!update.Succeeded)
                {
                    foreach(var e in update.Errors)
                    {
                        throw new Exception(e.Description); 
                    }
                }
                var result = _userManager.FindByIdAsync(userUpdate.Result.Id.ToString());
                return result.Result;
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
