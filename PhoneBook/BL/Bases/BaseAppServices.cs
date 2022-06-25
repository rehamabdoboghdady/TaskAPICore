using AutoMapper;
using BL.Configuration;
using BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Bases
{
    public class BaseAppServices : IDisposable
    {
        protected IUnitOfWork TheUnitOfWork { get; set; }
        protected readonly IMapper Mapper = AutoMapperProfile.mapp;
        public BaseAppServices(IUnitOfWork theUnitOfWork)
        {
            TheUnitOfWork = theUnitOfWork;
        }
        public void Dispose()
        {
            TheUnitOfWork.Dispose();
        }
    }
}
