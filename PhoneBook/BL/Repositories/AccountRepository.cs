using BL.Bases;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class AccountRepository : BaseRepository<User>
    {
        private readonly UserManager<User> manager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountRepository(DbContext db, UserManager<User> manager, RoleManager<IdentityRole> roleManager) : base(db)
        {
            //manager = new ApplicationUserManager(db);
            this.manager = manager;
            this.roleManager = roleManager;
        }
        public User GetAccountById(string id)
        {
            return GetFirstOrDefault(l => l.Id == id);
        }
        public List<User> GetAllAccounts()
        {
            return GetAll().ToList();
        }
        public async Task<User> FindByName(string userName)
        {
            User result = await manager.FindByNameAsync(userName);
            return result;
        }
        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            var userRoles = await manager.GetRolesAsync(user);
            return userRoles;
        }

        public async Task<User> FindById(string id)
        {
            User result = await manager.FindByIdAsync(id);
            return result;
        }
        public async Task<User> Find(string email, string password)
        {
            var user = await manager.FindByEmailAsync(email);
            if (user != null && await manager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            return null;
        }
        public async Task<IdentityResult> Register(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            IdentityResult result;
            result = await manager.CreateAsync(user, user.Password);
            return result;
        }
        public async Task<IdentityResult> AssignToRole(string userid, string rolename)
        {
            var user = await manager.FindByIdAsync(userid);
            if (user != null && await roleManager.RoleExistsAsync(rolename))
            {
                IdentityResult result = await manager.AddToRoleAsync(user, rolename);
                return result;
            }
            return null;
        }
        public async Task<bool> updatePassword(User user)
        {
            manager.PasswordHasher.HashPassword(user, user.PasswordHash);
            IdentityResult result = await manager.UpdateAsync(user);
            return true;
        }
        public async Task<bool> UpdateAccount(User user)
        {
            IdentityResult result = await manager.UpdateAsync(user);
            return true;
        }
    }
}
