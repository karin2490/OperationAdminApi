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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService _accountService)
        {
            this._accountService = _accountService;
        }

        /// <summary>
        /// Get All Accounts service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Accounts/GetAll
        ///     {               
        ///     }
        ///
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>        
        /// <response code="500">There was an error with function</response>
        [HttpGet("Get-All")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetAll()
        {
            try
            {
                Response response = await _accountService.GetAllNumberAccountAsync(HttpContext);
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
        /// Get DropDown Accounts service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Accounts/GetAccountList
        ///     {               
        ///     }
        ///
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>        
        /// <response code="500">There was an error with function</response>
        [HttpGet("GetAccountList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetAccountList()
        {
            try
            {
                Response response = await _accountService.GetDropdownAccountListAsync(HttpContext);
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
        /// Get a account
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET    api/v1/Accounts/GetUsersByAccount/{accountId}     
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (Count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>
        [HttpGet("GetUsersByAccount/{accountId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetUsersByAccount(int accountId)
        {
            try
            {
                Response response = await _accountService.GetAllUserByAccountAsync(HttpContext,accountId);
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
        /// Account Create service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Account/Create
        ///     {       
        ///         "AccountName": "string",
        ///         "AccountDescript": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="accountReq"></param>        
        /// <returns></returns>
        /// <response code="200"> Added action</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="406"> Data can't be empty or null (Name)</response>       
        /// <response code="500"> There was an error with function</response>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Create(R.AccountRequest accountReq)
        {
            try
            {
                Response response = await _accountService.InsertAccountAsync(HttpContext, accountReq);
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
        ///    POST api/v1/Account/Update
        ///     {   
        ///          "AccountId": 0,
        ///         "AccountName": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="accountReq"></param>        
        /// <returns></returns>
        /// <response code="200">Update action</response>  
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="404"> Data not found (Id)</response> 
        /// <response code="406"> Data can't be empty or null (Name)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("{update}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Update(R.AccountRequest accountReq)
        {
            try
            {
                Response response = await _accountService.UpdateAccountAsync(HttpContext,accountReq);
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
        /// Account Delete service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/v1/Account/Delete/{accountId}
        ///     {               
        ///     }
        ///
        /// </remarks>
        /// <param name="accountId"></param>        
        /// <returns></returns>
        /// <response code="200">Delete company</response>      
        /// <response code="401"> Unauthorized: You need a token or valid token</response>  
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpDelete("delete/{accountId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Delete(int accountId)
        {
            try
            {
                if (accountId <= 0)
                {
                    return BadRequest();
                }

                Response response = await _accountService.DeleteAccountAsync(HttpContext, accountId);
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
        /// Active Account service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Account/Active/{accountId}
        ///     {      
        ///     }
        ///
        /// </remarks>
        /// <param name="accountId"></param>        
        /// <returns></returns>
        /// <response code="200">Active company</response>   
        /// <response code="401"> Unauthorized: You need a token or valid token</response>  
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpDelete("active/{accountId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Active(int accountId)
        {
            try
            {
                if (accountId <= 0)
                {
                    return BadRequest();
                }

                Response response = await _accountService.ActiveAccountAsync(HttpContext, accountId);
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
        /// Active Account service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Account/Inactive/{accountId}
        ///     {      
        ///     }
        ///
        /// </remarks>
        /// <param name="accountId"></param>        
        /// <returns></returns>
        /// <response code="200">Active company</response>   
        /// <response code="401"> Unauthorized: You need a token or valid token</response>  
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpDelete("inactive/{accountId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Inactive(int accountId)
        {
            try
            {
                if (accountId <= 0)
                {
                    return BadRequest();
                }

                Response response = await _accountService.InactiveAccountAsync(HttpContext, accountId);
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
