using DAL;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public static class DomainDIExtensions
    {
        public static void AddDomainDataServices(this IServiceCollection services, string conectionString)
        {
            services.AddDbContext<CardDbContext>(options =>
            {
                options.UseSqlServer(conectionString);
            },
             ServiceLifetime.Transient);
            services.AddTransient<IUnitOfWork, UnitOfWorks>();
        }
    }
}
