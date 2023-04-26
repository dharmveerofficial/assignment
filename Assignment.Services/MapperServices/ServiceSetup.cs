using Assignment.Identity.IServices;
using Assignment.Services.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Assignment.Services.MapperServices
{
    public static class ServiceSetup
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services) => services
               .AddAutoMapper(Assembly.GetExecutingAssembly())
               .AddScoped<ICRUDServices, CRUDServices>();
    }
}
