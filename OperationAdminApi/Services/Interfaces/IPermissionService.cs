using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.Services.Interfaces
{
    public interface IPermissionService
    {
      
        Task<Response> GetDropDownPermissionAsync(HttpContext context);
    }
}
