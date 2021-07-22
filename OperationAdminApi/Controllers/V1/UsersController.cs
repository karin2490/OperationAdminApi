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
using ENUM = OperationAdminDB.Enum;
using R = OperationAdminApi.CommonObjects.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OperationAdminApi.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v1/users")]
    [ApiExplorerSettings(GroupName = "V1")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService _userService)
        {
            this._userService = _userService;
        }

        /// <summary>
        /// Get All User service
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET: api/v1/UsersController/GetAll
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (Count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>
        [HttpGet("get-all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Utils.AuthorizeAction(ENUM.Module.USER, ENUM.Permission.READ)]
        public async Task<ActionResult<IResponse>> GetAll(int page = 0, int quantity = 0)
        {
            try
            {
                Response response = await _userService.GetAllNumberUserAsync(HttpContext);
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
        /// Get All User service
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET: api/v1/UsersController/GetUser
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (Count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Utils.AuthorizeAction(ENUM.Module.USER, ENUM.Permission.READ)]
        public async Task<ActionResult<IResponse>> GetUser(int id)
        {
            try
            {
                Response response = await _userService.GetUserAsync(HttpContext, id);
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
        /// User Create service
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST api/v1/Users/Update
        ///     {       
        ///         "UserId": 0,
        ///         "FirstName": "string",
        ///         "LastName": "string",
        ///         "Email": "string",
        ///         "RoleId": 0,
        ///         "AccountId": 0,      
        ///         "AdmissionDate":"string",
        ///         "Status": true
        ///     }
        /// </remarks> 
        ///
        /// <returns></returns>
        /// <response code="200">Number of records (Count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="406">User email invalid or User email duplicate</response>
        /// <response code="500">There was an error with function</response>
        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Utils.AuthorizeAction(ENUM.Module.USER, ENUM.Permission.CREATE)]
        public async Task<ActionResult<IResponse>> Create(R.UserRequest userRequest)
        {
            try
            {
                Response response = await _userService.InsertUserAsync(HttpContext, userRequest);
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
        ///     POST api/v1/Users/Update
        ///     {       
        ///         "UserId": 0,
        ///         "FirstName": "string",
        ///         "LastName": "string",
        ///         "Email": "string",
        ///         "RoleId": 0,
        ///         "AccountId": 0,      
        ///         "AdmissionDate":"string",
        ///         "Status": true
        ///     }
        ///
        /// </remarks>
        /// <param name="userRequest"></param>        
        /// <returns></returns>
        /// <response code="200">Update action</response>  
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="404"> Data not found (Id)</response> 
        /// <response code="406"> Data can't be empty or null (Name)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("{update}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Utils.AuthorizeAction(ENUM.Module.USER, ENUM.Permission.UPDATE)]
        public async Task<ActionResult<IResponse>> Update(R.UserRequest userRequest)
        {
            try
            {
                Response response = await _userService.UpdateUserAsync(HttpContext, userRequest);
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

        [HttpPut("inactive/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Utils.AuthorizeAction(ENUM.Module.USER, ENUM.Permission.UPDATE)]
        public async Task<ActionResult<IResponse>> Inactive(int userId)
        {
            try
            {
                Response response = await _userService.InactiveUserAsync(HttpContext, userId);
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

        [HttpPut("active/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Utils.AuthorizeAction(ENUM.Module.USER, ENUM.Permission.UPDATE)]
        public async Task<ActionResult<IResponse>> Active(int userId)
        {
            try
            {
                Response response = await _userService.ActiveUserAsync(HttpContext, userId);
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

        // DELETE api/<UsersController>/5
        [HttpDelete("delete/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Utils.AuthorizeAction(ENUM.Module.USER, ENUM.Permission.DELETE)]
        public async Task<ActionResult<IResponse>> Delete(int id)
        {
            try
            {
                Response response = await _userService.DeleteUserAsync(HttpContext, id);
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
