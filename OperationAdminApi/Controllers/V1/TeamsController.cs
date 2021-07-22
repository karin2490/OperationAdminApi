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
    public class TeamsController : ControllerBase
    {

        private readonly ITeamService _teamsService;

        public TeamsController(ITeamService _teamsService)
        {
            this._teamsService = _teamsService;
        }


        /// <summary>
        /// Get All Teams service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Teams/GetAll
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
                Response response = await _teamsService.GetAllNumberAsync(HttpContext);
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
        /// Get All Teams service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Teams/GetTeamsList
        ///     {               
        ///     }
        ///
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>        
        /// <response code="500">There was an error with function</response>
        [HttpGet("GetTeamsList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetTeamsList()
        {
            try
            {
                Response response = await _teamsService.GetDropdownTeamListAsync(HttpContext);
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
        /// Get All Teams service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Teams/GetAllWithAccounts
        ///     {               
        ///     }
        ///
        /// </remarks>        
        /// <returns></returns>
        /// <response code="200">Number of records (count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>        
        /// <response code="500">There was an error with function</response>
        [HttpGet("GetAllWithAccounts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetAllWithAccounts()
        {
            try
            {
                Response response = await _teamsService.GetAllWithAccountsAsync(HttpContext);
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
        /// Get accounts of a team
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET    api/v1/Teams/GetAccountsOfATeam/{teamID}     
        /// </remarks>        
        ///  /// <param name="teamID"></param>  
        /// <returns></returns>
        /// <response code="200">Number of records (Count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>
        [HttpGet("GetAccountsOfATeam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetAccountsOfATeam(int teamID)
        {
            try
            {
                Response response = await _teamsService.GetAllAccountsOfATeamById(HttpContext, teamID);
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
        /// Get a team
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET    api/v1/Teams/{teamID}     
        /// </remarks>        
        ///  /// <param name="teamID"></param>  
        /// <returns></returns>
        /// <response code="200">Number of records (Count)</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="500">There was an error with function</response>
        [HttpGet("{teamID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> GetTeamById(int teamID)
        {
            try
            {
                Response response = await _teamsService.GetByIdAsync(HttpContext, teamID);
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
        /// Teams Create service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Teams/Create
        ///     {       
        ///         "TeamName": "string",
        ///         "Status": true
        ///     }
        ///
        /// </remarks>
        /// <param name="teamReq"></param>        
        /// <returns></returns>
        /// <response code="200"> Added action</response>
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="406"> Data can't be empty or null (Name)</response>       
        /// <response code="500"> There was an error with function</response>
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Create(R.TeamRequest teamReq)
        {
            try
            {
                Response response = await _teamsService.InsertTeamAsync(HttpContext, teamReq);
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
        /// Teams Update service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/Teams/Update
        ///     {       
        ///         "Id": 0,
        ///         "Name": "string",
        ///         "Description": "string",
        ///         "Id": 0,        
        ///     }
        ///
        /// </remarks>
        /// <param name="teamReq"></param>        
        /// <returns></returns>
        /// <response code="200">Update action</response>  
        /// <response code="401"> Unauthorized: You need a token or valid token</response>
        /// <response code="404"> Data not found (Id)</response> 
        /// <response code="406"> Data can't be empty or null (Name)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Update(R.TeamRequest teamReq)
        {
            try
            {
                Response response = await _teamsService.UpdateTeamAsync(HttpContext, teamReq);
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
        /// Teams Delete service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/v1/Teams/Delete/{teamId}
        ///     {               
        ///     }
        ///
        /// </remarks>
        /// <param name="teamId"></param>        
        /// <returns></returns>
        /// <response code="200">Delete company</response>      
        /// <response code="401"> Unauthorized: You need a token or valid token</response>  
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpDelete("{teamId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Delete(int teamId)
        {
            try
            {
                Response response = await _teamsService.DeleteTeamAsync(HttpContext, teamId);
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
        /// Active Teams service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Teams/Active/{Id}
        ///     {      
        ///     }
        ///
        /// </remarks>
        /// <param name="teamId"></param>        
        /// <returns></returns>
        /// <response code="200">Active company</response>   
        /// <response code="401"> Unauthorized: You need a token or valid token</response>  
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("Active")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Active(int teamId)
        {
            try
            {
                Response response = await _teamsService.ActiveTeamAsync(HttpContext, teamId);
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
        /// Inactive Teams service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/Teams/Inactive/{Id}
        ///     {      
        ///     }
        ///
        /// </remarks>
        /// <param name="teamId"></param>        
        /// <returns></returns>
        /// <response code="200">Active company</response>   
        /// <response code="401"> Unauthorized: You need a token or valid token</response>  
        /// <response code="404"> Data not found (Id)</response>
        /// <response code="500">There was an error with function</response>
        [HttpPut("Inactive")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IResponse>> Inactive(int teamId)
        {
            try
            {
                Response response = await _teamsService.InactiveTeamAsync(HttpContext, teamId);
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
