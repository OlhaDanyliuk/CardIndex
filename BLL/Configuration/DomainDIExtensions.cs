using DAL;
using DAL.Entities;
using DAL.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Configuration
{
    public static class DomainDIExtensions
    {
        public static void AddDomainDataServices(this IServiceCollection services, string conectionString)
        {
            services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole<long>>()
                .AddRoleManager<RoleManager<IdentityRole<long>>>()
                .AddEntityFrameworkStores<CardDbContext>();
            

            services.AddDbContext<CardDbContext>(options =>
            {
                options.UseSqlServer(conectionString);
            },
             ServiceLifetime.Transient);
            services.AddTransient<IUnitOfWork, UnitOfWorks>();
        }
    }
}
