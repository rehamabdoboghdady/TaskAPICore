using AutoMapper;
using BL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Configuration
{
    public class AutoMapperProfile
    {
        public static IMapper mapp { get; set; }
        static AutoMapperProfile()
        {

            var config = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Contact, ContactViewModel>().ReverseMap();
                cfg.CreateMap<User, LoginViewModel>().ReverseMap();
                cfg.CreateMap<User, RegisterViewModel>().ReverseMap();
            });
            mapp = config.CreateMapper();

        }
    }
}