using Assignment.Data.Models;
using Assignment.Identity.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Assignment.Services.MapperServices
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
            CreateMap<Setting, SettingViewModel>();
        }
    }
}
