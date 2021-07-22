using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OperationAdminApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENUM = OperationAdminDB.Enum;
using R = OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common.Enum;
using OperationAdminRepository.Common;
using NLog.Fluent;
using OperationAdminRepository;

namespace OperationAdminApi.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "V1")]
    [ApiController]
    [Authorize]
    //public class AuthController : CoreBaseController<AuthController>
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService _authService)
        {
            this._authService = _authService;
        }

        /// <summary>
        /// Login service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Login
        ///     {       
        ///         "email": "user@example.com"
        ///         "pin": "password",
        ///     }
        ///
        /// </remarks>
        /// <param name="authRequest">Auth</param>        
        /// <returns></returns>
        /// <response code="200"> When it's right Message = Valid User</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="406"> User does not exist Message = Invalid User and this is another Message = Invalid password</response>        
        /// <response code="500"> Fatal error Message = Error of function</response>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(R.AuthorizationRequest authRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                    Response response = await _authService.LoginAsync(HttpContext, authRequest);
                    return response.Type == ResponseType.SUCCESS ? Ok(response) : StatusCode((int)response.Type, response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }



        /// <summary>
        /// Refresh Token service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Refresh/{token}
        ///     {       
        ///        
        ///     }
        ///
        /// </remarks>
        /// <param name="tokenStr">Auth</param>        
        /// <returns></returns>
        /// <response code="200"> When it's right Message = Valid User</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="406"> User does not exist Message = Invalid User and this is another Message = Invalid password</response>        
        /// <response code="500"> Fatal error Message = Error of function</response>
        [HttpPost("Refresh/{tokenStr}")]
        [AllowAnonymous]
        public async Task<IActionResult> RefToken([FromRoute] string tokenStr)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                    Response response = await _authService.GetTokenAsync(HttpContext, tokenStr);
                    return response.Type == ResponseType.SUCCESS ? Ok(response) : StatusCode((int)response.Type, response);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.StackTrace);
                return StatusCode(500, ex.ToResponseExeption());
            }
        }

        /// <summary>
        /// Revoke Refresh Token
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post    /Revoke/{Id}     
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">When it's right Message = Token Revoked</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>   
        /// <response code="500"> Fatal error Message = Error of function</response>
        [HttpPost("Revoke/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> RevokeToken([FromRoute] int id)
        {
            try
            {
                if (id<=0)
                {
                    return BadRequest();
                }
                else
                {
                    Response response = await _authService.RevokeTokenAsync(HttpContext, id);
                    return response.Type == ResponseType.SUCCESS ? Ok(response) : StatusCode((int)response.Type, response);
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
