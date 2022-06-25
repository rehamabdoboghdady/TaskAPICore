using AutoMapper;
using BL.Bases;
using BL.Interfaces;
using BL.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
    public class RoleAppService :BaseAppServices
    {
        public RoleAppService(IUnitOfWork theUnitOfWork, IMapper mapper) : base(theUnitOfWork)
        {

        }
        public async Task CreateRoles()
        {
            await TheUnitOfWork.Role.CreateRoles();
        }
        public RoleViewModel GetRoleById(string id)
        {
            if (id == null || id == "")
                throw new ArgumentNullException();

            return Mapper.Map<RoleViewModel>(TheUnitOfWork.Role.GetRoleByID(id));
        }
        public IdentityResult Create(string rolename)
        {
            return TheUnitOfWork.Role.Create(rolename);
        }
        public async Task<IdentityResult> Update(RoleViewModel roleViewModel)
        {
            if (roleViewModel == null)
                throw new ArgumentNullException();
            if (roleViewModel.Id == null || roleViewModel.Id == string.Empty)
                throw new ArgumentException();

            var role = Mapper.Map<IdentityRole>(roleViewModel);
            return await TheUnitOfWork.Role.UpdateRole(role);
        }
        public bool DeleteRole(string id)
        {
            if (id == null || id == "")
                throw new ArgumentNullException();
            bool result = false;

            TheUnitOfWork.Role.DeleteRole(id);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }
        public bool AdminIfExists(string roleName)
        {
            if (roleName != null)
                return true;
            else
                return false;
        }
    }
}
