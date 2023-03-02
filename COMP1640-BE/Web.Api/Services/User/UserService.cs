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
using Web.Api.Services.FileUploadService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Entities.User> _userManager;
        protected AppDbContext context;
        private IPasswordHasher<Entities.User> _passwordHasher;
        private readonly IFileUploadService _fileUploadService;
        private RoleManager<Entities.Role> _roleManager;

        public UserService(UserManager<Entities.User> userManager, AppDbContext context, IPasswordHasher<Entities.User> passwordHasher, IFileUploadService fileUploadService, RoleManager<Entities.Role> roleManager)
        {
            _userManager = userManager;
            this.context = context;
            this._passwordHasher = passwordHasher;
            _fileUploadService = fileUploadService;
            _roleManager = roleManager;
        }

        public async Task<List<Entities.User>> GetAll()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Entities.User>> GetAllStaff()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("Staff");
                return users.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Entities.User>> GetAllAdminQA()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                roles.Remove(roles.Single(r => r.Name == "Staff"));
                List<Entities.User> result = new List<Entities.User>();
                foreach(var role in roles)
                {
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);
                    result.AddRange(users);
                }
                return result;
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
                var user = await _userManager.FindByIdAsync(id.ToString());
                return user;
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
                //Check user is existed
                var checkUser = await _userManager.FindByIdAsync(id.ToString());         
                if (checkUser == null)
                {
                    throw new Exception("Can not find the user");
                }
                var userUpdate = checkUser;
                //Check email and username is existed
                var users = await _userManager.Users.ToListAsync();
                foreach (var item in users)
                {
                    if(item.Id != id)
                    {
                        if (item.Email.ToLower().Trim() == user.Email.ToLower().Trim())
                        {
                            throw new Exception("The email has been used, please choose another email!");
                        }
                        if (item.UserName.ToLower().Trim() == user.UserName.ToLower().Trim())
                        {
                            throw new Exception("The username has been used, please choose another username!");
                        }
                    }
                }
                //Add update data
                userUpdate.UserName = user.UserName;
                userUpdate.Email = user.Email;
                userUpdate.Name = user.Name;
                userUpdate.PasswordHash = _passwordHasher.HashPassword(userUpdate, user.Password);
                userUpdate.Address = user.Address;
                userUpdate.DepartmentId = user.DepartmentId;
                userUpdate.PhoneNumber = user.PhoneNumber;
                
                if (user.File != null)
                {
                    var imageUploadResult = await _fileUploadService.UploadImageAsync(user.File);
                    if (imageUploadResult.Error != null)
                        throw new Exception(imageUploadResult.Error.Message);

                    if (!string.IsNullOrEmpty(userUpdate.PublicId))
                        await _fileUploadService.DeleteMediaAsync(userUpdate.PublicId);

                    userUpdate.Avatar = imageUploadResult.SecureUrl.ToString();
                    userUpdate.PublicId = imageUploadResult.PublicId;
                }
                //Validate and update role
                var userRole = await _userManager.GetRolesAsync(checkUser);
                if(user.Role != null)
                {
                    if (userRole.Any())
                    {
                        if (userRole[0].Trim().ToLower() != user.Role.Trim().ToLower())
                        {
                            await _userManager.RemoveFromRoleAsync(checkUser, userRole[0]);
                        }
                    }
                    await _userManager.AddToRoleAsync(userUpdate, user.Role);
                }
                //update user and their role
                var update = await _userManager.UpdateAsync(userUpdate);          
                if (!update.Succeeded)
                {
                    foreach(var e in update.Errors)
                    {
                        throw new Exception(e.Description); 
                    }
                }
                var result = await _userManager.FindByIdAsync(userUpdate.Id.ToString());
                return result;
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
                if (!string.IsNullOrEmpty(user.PublicId))
                    await _fileUploadService.DeleteMediaAsync(user.PublicId);

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
