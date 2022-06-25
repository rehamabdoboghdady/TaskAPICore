using BL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        int Commit();
        RoleRepository Role { get; }
        AccountRepository Account { get; }
        ContactRepository Contact { get; }
    }
          
        
}
