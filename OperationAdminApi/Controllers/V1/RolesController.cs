using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using OperationAdminApi.Services.Interfaces;
using OperationAdminRepository;
using OperationAdminRepository.Common;
using OperationAdminRepository.Common.Enum;
using OperationAdminRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R = OperationAdminApi.CommonObjects.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OperationAdminApi.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "V1")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IRolesService _rolesService;

        public RolesController(IRolesService _rolesService)
        {
            this._rolesService = _rolesService;
        }


        /// <summary>
        /// Get All User service
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET: api/v1/RolesControlles/GetRolesList
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (Count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>
        [HttpGet("GetRolesList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetRolesList(int page = 0, int quantity = 0)
        {
            try
            {
                Response response = await _rolesService.GetDropDownRolesAsync(HttpContext);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

        /// <summary>
        /// Get a specific userType
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Roles/{roleId}
        ///     {               
        ///     }
        ///
        /// </remarks>         
        /// <returns></returns>
        /// <response code="200"> Number of records (count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>        
        /// <response code="500"> There was an error with function</response>
        [HttpGet("{roleId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetRole(int roleId)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                Response response = await _rolesService.GetRoleById(HttpContext, id);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

        /// <summary>
        /// Get User Type Function service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Roles/GetModuleByRole/{Id}
        ///     {                    
        ///     }
        ///
        /// </remarks>
        /// <param name="roleId">Auth</param>        
        /// <returns></returns>
        /// <response code="200">Number of records (count)</response>     
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>
        [HttpGet("GetModuleByRole/{roleId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetModuleByRole(int roleId)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                Response response = await _rolesService.GetModuleByRoleId(HttpContext, id);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch(Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

            /// <summary>
            /// User Type Create service
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            ///     POST api/v1/Roles/Update
            ///     {       
            ///         "RoleId": 0,
            ///         "RoleDescription": "string",
            ///         "Status": true
            ///     }
            ///
            /// </remarks>
            /// <param name="roleRequest">Auth</param>        
            /// <returns></returns>
            /// <response code="200">Add new record</response>
            /// <response code="401"> Unauthorized: You need a token or valid token</response>
            /// <response code="406"> Data can't be empty or null (Description)</response>
            /// <response code="500">There was an error with function</response>
            [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Create(R.RoleRequest roleRequest)
        {
            try
            {
                Response response = await _rolesService.InsertRoleNameAsync(HttpContext, roleRequest);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

        /// <summary>
        /// Account Update service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Roles/Update
        ///     {       
        ///         "RoleId": 0,
        ///         "RoleDescription": "string",
        ///         "Status": true
        ///     }
        ///
        /// </remarks>
        /// <param name="roleRequest"></param>        
        /// <returns></returns>
        /// <response code="200">Update action</response>  
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="404"> Data not found (Id)</response> 
        /// <response code="406"> Data can't be empty or null (Name)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("{update}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Update(R.RoleRequest roleRequest)
        {
            try
            {
                Response response = await _rolesService.UpdateRoleAsync(HttpContext, roleRequest);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

        /// <summary>
        /// User Type Inactive service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Roles/Inactive/{roleId}
        ///     {                      
        ///     }
        ///
        /// </remarks>
        /// <param name="roleId"></param>        
        /// <returns></returns>
        /// <response code="200">Inactive UserType</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>   
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("inactive/{roleId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Inactive(int roleId)
        {
            try
            {
                Response response = await _rolesService.InactiveRoleAsync(HttpContext, id);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

        /// <summary>
        /// User Type Inactive service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Roles/Active/{roleId}
        ///     {                      
        ///     }
        ///
        /// </remarks>
        /// <param name="roleId"></param>        
        /// <returns></returns>
        /// <response code="200">Inactive UserType</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>   
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("active/{roleId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Active(int roleId)
        {
            try
            {
                Response response = await _rolesService.ActiveRoleAsync(HttpContext, id);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

        /// <summary>
        /// User Type Delete service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/v1/Roles/Delete/{roleId}
        ///     {                       
        ///     }
        ///
        /// </remarks>
        /// <param name="roleId">Auth</param>        
        /// <returns></returns>
        /// <response code="200">Delete UserType</response> 
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpDelete("delete/{roleId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Delete(int roleId)
        {
            try
            {
                Response response = await _rolesService.DeleteRoleAsync(HttpContext, id);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }


        /// <summary>
        /// Add Function To User Type service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Roles/SetModuleByRole
        ///     {               
        ///         "ModuleByRoleId": 0
        ///         "RoleId":0
        ///         "ModuleId":0
        ///         
        ///     }
        ///
        /// </remarks>
        /// <param name="moduleBYroleRequest">Auth</param>        
        /// <returns></returns>
        /// <response code="200">Added features</response>                
        /// <response code="401"> Unauthorized: You need a token or valid token</response>        
        /// <response code="500">There was an error with function</response>
        [HttpPut("SetModuleByRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> SetModuleByRole(R.ModuleByRoleRequest moduleBYroleRequest)
        {
            try
            {
                Response response = await _rolesService.SetModuleByRole(HttpContext, moduleBYroleRequest);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }



        [HttpPut("SetModulePermissions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> SetModuleByRole(R.PermissionOnModuleRequest permOnModuleReq)
        {
            try
            {
                Response response = await _rolesService.SetModulePermissions(HttpContext, permOnModuleReq);
                if (response.Type != ResponseType.SUCCESS)
                {
                    return StatusCode((int)response.Type, response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }
    }
}
