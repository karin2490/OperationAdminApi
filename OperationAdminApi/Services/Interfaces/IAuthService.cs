using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.Services.Interfaces
{
   public interface IAuthService
    {
        Task<Response> LoginAsync(HttpContext context, AuthorizationRequest Auth);

        Task<Response> GetTokenAsync(HttpContext context, string refreshToken);
        Task<Response> RevokeTokenAsync(HttpContext context, int Id);
       

    }
}
