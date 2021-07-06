using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.Infraestructure.Repository
{
    public static class RepositoryRegistry
    {
        public static IServiceCollection Add_RepositoryRegistry(this IServiceCollection services)
        {
           
            services.AddScoped<AccountRepository>();
           
            services.AddScoped<PermissionRepository>();
            services.AddScoped<ModuleByRoleRepository>();
          
           
            services.AddScoped<RolesRepository>();
            services.AddScoped<TeamsLogRepository>();
            services.AddScoped<TeamsRepository>();
            services.AddScoped<UserProfileRepository>();
            services.AddScoped<UsersRepository>();
            
            return services;
        }
    }
}
