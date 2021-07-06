using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.Services.Interfaces
{
    public interface IProfileService
    {
      
        Task<Response> GetProfileByIdAsync(HttpContext context, int userId, int id);
        Task<Response> AddProfileAsync(HttpContext context, UserProfileRequest request);
        Task<Response> UpdateProfileAsync(HttpContext context, UserProfileRequest request);
        Task<Response> DeleteProfileAsync(HttpContext context, int userId);
    
    }
}
