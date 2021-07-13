using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminRepository.Repository;
using OperationAdminDB.Data;
using Microsoft.EntityFrameworkCore;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class TokenRepository:RepositoryBase<M.Token>
    {
        private readonly OperationAdminContext DBCon;

        public TokenRepository(OperationAdminContext DBContext)
            :base(DBContext)
        {
            this.DBCon = DBContext;
        }


        public async Task<bool> isValid(string token)
        {
            if(await DBCon.Tokens.AnyAsync(x => x.TokenStr== token))
            {
                M.Token rToken = await DBCon.Tokens.Where(a => a.TokenStr == token && a.Revoked == false)
                    .OrderByDescending(a => a.TokenId).FirstOrDefaultAsync();
                if (rToken != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> revokeToken(int id)
        {
            if (await DBCon.Tokens.AnyAsync(x => x.TokenId==id))
            {
                M.Token rToken = await DBCon.Tokens.Where(a => a.TokenId == id && a.Revoked == false)
                    .OrderByDescending(a => a.TokenId).FirstOrDefaultAsync();
                if (rToken != null)
                {
                    rToken.Revoked = true;
                    DBCon.Update(rToken);
                    DBCon.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public async Task<List<D.TokensDTO>> GetAllTokensActive()
        {
            var rTokens = await DBCon.Tokens
                .Where(x => x.Revoked == false)
                .Select(a => new D.TokensDTO()
                { 
                    Tokenid=a.TokenId,
                    Email=a.Email,
                    Revoked=a.Revoked

                }).ToListAsync();
            return rTokens;
        }


    }
}
