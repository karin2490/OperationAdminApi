
using Microsoft.Extensions.DependencyInjection;
using OperationAdminApi.Services.Implementations;
using OperationAdminApi.Services.Interfaces;

namespace OperationAdminApi.Services
{
    public static class ServicesRegistry
    {
        public static IServiceCollection Add_ServicesRegistry(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ITeamByUserService, TeamByUserService>();
            services.AddScoped<ITeamLogService, TeamLogService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
