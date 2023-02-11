using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using System.Linq;
using System.Web.Http.ModelBinding;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web.Api.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private RoleManager<IdentityRole<Guid>> roleManager;

        public RoleService(IUnitOfWork unitOfWork, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _unitOfWork = unitOfWork;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityRole<Guid>>> GetAll()
        {
            try
            {
                var role = roleManager.Roles.AsEnumerable();
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> Create(string roleName)
        {
            try
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                return result;
            }
            catch (Exception )
            {
                throw;
            }
        }
        public async Task<IdentityResult> Update(Guid id, string updateRole)
        {
            try
            {
                var idUpdate = id.ToString();
                var role = await roleManager.FindByIdAsync(idUpdate);
                if(role==null)
                {
                    throw new Exception("This role does not exist!");
                }
                role.Name = updateRole;
                var result = await roleManager.UpdateAsync(role);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IdentityResult> Delete(string roleName)
        {
            try
            {
                var role = await roleManager.FindByNameAsync(roleName);
                var result = await roleManager.DeleteAsync(role);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
