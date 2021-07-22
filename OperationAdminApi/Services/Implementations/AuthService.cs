using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NLog.Fluent;
using OpAdminRepository.Common.Cache;
using OperationAdminApi.CommonObjects.DTOs;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminApi.Infraestructure.Repository;
using OperationAdminApi.Services.Interfaces;
using OperationAdminRepository;
using OperationAdminRepository.Common;
using OperationAdminRepository.Common.Enum;
using OperationAdminRepository.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OperationAdminApi.Services.Implementations
{
    public class AuthService:IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AuthRepository _authRepository;
        private readonly TokenRepository _tokenRepository;
        Response response { get; set; }

        public AuthService(IConfiguration _configuration, TokenRepository _tokenRepository,
            AuthRepository _authRepository)
        {
            this._configuration = _configuration;
            this._tokenRepository = _tokenRepository;
            this._authRepository = _authRepository;
            this.response = new Response();
        }

        public async Task<Response> GetTokensAsync(HttpContext context)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _tokenRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    List<TokensDTO> tokens = await _tokenRepository.GetAllTokensActive();
                    return tokens.ToResponse(string.Format("Number of records {0}", tokens.Count));
                }
                else
                {
                    return "".ToResponse(false, ResponseType.UNAUTHORIZED, "Unauthorized action");
                }

            }
            catch(Exception ex)
            {
                Log.Error($"Some Error happened on Auth Service GetTokensAsync Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> LoginAsync(HttpContext context, AuthorizationRequest authRequest)
        {
            try
            {
                var result = await _authRepository.GetloginAync(authRequest);
                if (result == null)
                {
                    return result.ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Invalid User");
                }
                else
                {
                    //if(authRequest.Email=="root@arkusnexus.com" && authRequest.Pin=="1234")
                    //{
                    //    var token = await BuildToken(result);
                    //    return token.ToResponse("Valid User");

                    //}
                    var userPin = MD5.Decrypt(result.PassEncrypted);
                    if (userPin == authRequest.Pin)
                    {
                        var token = await BuildToken(result);
                        return token.ToResponse("Valid User");
                    }
                    else
                    {
                        return "".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Invalid password");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Some Error happened on Auth Service LoginAsync Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> GetTokenAsync(HttpContext context, string rToken)
        {
            try
            {
                rToken = MD5.Encrypt(rToken);
                var _rtoken = await _authRepository.VerifyTokenAsync(rToken);

                if (rToken == null  || ! await _tokenRepository.isValid(rToken))
                {
                    return "".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Invalid rToken");
                }
                var TokenAndRtoken = await GenerateRefToken(rToken);
                if (TokenAndRtoken == null)
                {
                    return "".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Invalid Ref Token");
                }
                return TokenAndRtoken.ToResponse("Valid Refresh Token");
            }
            catch (Exception ex)
            {
                Log.Error($"Some Error happened on Auth Service GetTokensAsync Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> RevokeTokenAsync(HttpContext context, int Id)
        {
            try
            {
                if (!await _tokenRepository.revokeToken(Id))
                {
                    return "".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Invalid Refresh Token");
                }
                return "".ToResponse("Token revoked sucessfully");
            }
            catch (Exception ex)
            {
                Log.Error($"Some Error happened on Auth Service RevokeTokenAsync Ex: {ex}");
                throw ex;
            }
        }



        private async Task<UserToken> BuildToken(M.User userInfo)
        {
            var profile = await _authRepository.GetUserAsync(userInfo);
            List<string> roles = await _authRepository.GetRolesByUserAsync(userInfo.UserId);
            AuthDTO auth = CreateAuthRequest(profile, roles);

            List<ModulePermissionsDTO> userPermission = await _authRepository.GetPermissionOnModuleByUserIdAsync(auth.UserId);
            UserCache userCachePermission = CreateCache(auth, userPermission);
            string AuthString = auth.Serializer();
            string userCachePermissionString = userCachePermission.Serializer();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, AuthString),
                new Claim("UserDataPermission", userCachePermissionString),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int TimeExpiration = int.Parse(_configuration["JWT:ExpiraToken"]);
            var expiration = DateTime.Now.AddMilliseconds(TimeExpiration);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            var rtoken = await _authRepository.GetTokenAsync(profile);
            if (rtoken!=null)
            {
                rtoken.Email = userInfo.Email;
                rtoken.TokenStr = MD5.Encrypt(Guid.NewGuid().ToString());
                rtoken.Revoked = false;
            }
            else
            {
                rtoken = new M.Token
                {
                    Email = userInfo.Email,
                    TokenStr = MD5.Encrypt(Guid.NewGuid().ToString()),
                    Revoked = false
                };
            }

            _tokenRepository.Update(rtoken);
            await _tokenRepository.SaveAsync();

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                rTokenStr = MD5.Decrypt(rtoken.TokenStr),
                Expiration = expiration
            };
        }

        private async Task<UserToken> GenerateRefToken(string oldRefToken)
        {
            var refToken = await _authRepository.VerifyTokenAsync(oldRefToken);
            var profile = await _authRepository.GetProfileFromTokenAsync(refToken);
            List<string> roles = await _authRepository.GetRolesByUserAsync(profile.UserId);
            AuthDTO auth = CreateAuthRequest(profile, roles);

            List<ModulePermissionsDTO> rolesDTO = await _authRepository.GetPermissionOnModuleByUserIdAsync(auth.UserId);
            UserCache userCachePermission = CreateCache(auth, rolesDTO);
            string AuthString = auth.Serializer();
            string userCahePermissionString = userCachePermission.Serializer();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, AuthString),
                new Claim("UserDataPermission", userCahePermissionString),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int TimeExpiration = int.Parse(_configuration["JWT:ExpiraToken"]);
            var expiration = DateTime.Now.AddMilliseconds(TimeExpiration);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            refToken.TokenStr = Guid.NewGuid().ToString();
            refToken.TokenStr = MD5.Encrypt(refToken.TokenStr);
            _tokenRepository.Update(refToken);
            await _tokenRepository.SaveAsync();

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                rTokenStr = MD5.Decrypt(refToken.TokenStr),
                Revoked = false,
                Expiration = expiration
            };
        }

        private AuthDTO CreateAuthRequest(M.User userInfo, List<string> roles)
        {
            AuthDTO auth = new AuthDTO();
            auth.UserId = userInfo.UserId;
            auth.Email = userInfo.Email;
            auth.FirstName = userInfo.FirstName;
            auth.LastName = userInfo.LastName;
            auth.Roles = roles;
            return auth;
        }

        private UserCache CreateCache(AuthDTO authDTO, List<ModulePermissionsDTO> userPermission)
        {
            UserCache userCache = new UserCache();
            userCache.UserId = authDTO.UserId;
            userCache.Email = authDTO.Email;
            userCache.FirtsName = authDTO.FirstName;
            userCache.LastName = authDTO.LastName;
            userCache.Roles = authDTO.Roles;
            userCache.PermissionCache = new List<Cache_PermissionsOnModule>();
            userCache.PermissionCache.Clear();

            userPermission.ForEach(a =>
            {
                userCache.PermissionCache.Add(new Cache_PermissionsOnModule(a.ModuleId, a.PermissionsId));
            });

            return userCache;
        }

        
    }
}
