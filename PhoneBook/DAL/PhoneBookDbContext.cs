using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ApplicationDbContext :IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseSqlServer("Data Source=.;Initial Catalog=PhoneBook;Integrated Security=True"
            , options => options.EnableRetryOnFailure());
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Contact> contacts { get; set; }
    }
    public class ApplicationUserStore : UserStore<User>
    {
        public ApplicationUserStore() : base(new ApplicationDbContext())
        {

        }
        public ApplicationUserStore(DbContext db) : base(db)
        {

        }
    }
}
