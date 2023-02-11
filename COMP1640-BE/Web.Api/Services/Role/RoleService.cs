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
using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Web.Api.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IMapper _mapper;
        public RoleService(IUnitOfWork unitOfWork, RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<List<RoleResponseModel>> GetAll()
        {
            try
            {
                var role = await roleManager.Roles.ToListAsync();
                var result = _mapper.Map<List<RoleResponseModel>>(role);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoleResponseModel> Create(string roleName)
        {
            try
            {
                IdentityResult create = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if(!create.Succeeded)
                {
                    foreach(var e in create.Errors)
                    {
                        var error = e.Description;
                        throw new Exception(error);
                    }
                }
                IdentityRole<Guid> data = await roleManager.FindByNameAsync(roleName);
                var result = _mapper.Map<RoleResponseModel>(data);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<RoleResponseModel> Update(Guid id, string updateRole)
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
                var update = await roleManager.UpdateAsync(role);
                var data = await roleManager.FindByIdAsync(idUpdate);
                var result = _mapper.Map<RoleResponseModel>(data);
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
