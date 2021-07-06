using Microsoft.AspNetCore.Http;

using OperationAdminRepository.Common;
using OperationAdminApi.CommonObjects.Request;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Response> InsertAccountAsync(HttpContext context, AccountRequest request);
        Task<Response> UpdateAccountAsync(HttpContext context, AccountRequest request);
        Task<Response> DeleteAccountAsync(HttpContext context, int userId);
        Task<Response> GetAllNumberAccountAsync(HttpContext context);
        //Task<Response> GetAccountByUserAsync(HttpContext context, int userToConsultId);
        Task<Response> GetAllUserByAccountAsync(HttpContext context, int accountId);
        Task<Response> GetDropdownAccountListAsync(HttpContext context);
        Task<Response> ActiveAccountAsync(HttpContext context, int id);
        Task<Response> InactiveAccountAsync(HttpContext context, int id);
    }
}
