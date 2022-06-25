using BL.Interfaces;
using BL.Repositories;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Bases
{
   public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext EC_DbContext { get; set; }
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UnitOfWork(ApplicationDbContext EC_DbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this.EC_DbContext = EC_DbContext;
        }
        public int Commit()
        {
            return EC_DbContext.SaveChanges();
        }
        public void Dispose()
        {
            EC_DbContext.Dispose();
        }

        public ContactRepository contact;
        public ContactRepository Contact
        {
            get
            {
                if (contact == null)
                    contact = new ContactRepository(EC_DbContext);
                return contact;
            }
        }

        public AccountRepository account;
        public AccountRepository Account
        {
            get
            {
                if (account == null)
                    account = new AccountRepository(EC_DbContext, _userManager, _roleManager);
                return account;
            }
        }


        public RoleRepository role;
        public RoleRepository Role
        {
            get
            {
                return role;
            }
        }
    }
}
