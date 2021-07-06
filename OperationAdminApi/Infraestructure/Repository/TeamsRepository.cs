using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminDB.Data;
using Microsoft.EntityFrameworkCore;
using OperationAdminRepository.Repository;
using OperationAdminDB.Models;
using OpAdminRepository.Common.Cache;
using AutoMapper;
using System.Collections.Generic;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class TeamsRepository : RepositoryBase<M.Team>
    {
        private readonly OperationAdminContext DBCon;
        private readonly IMapper _mapper;
        public TeamsRepository(OperationAdminContext DBContext, IMapper _mapper)
            : base(DBContext)
        {
            this.DBCon = DBContext;
            this._mapper = _mapper;
        }

        public async Task<List<D.TeamWithAccount>> GetAllTeamsWAccount()
        {
            var teamswAccounts = await DBCon.Teams
                .Include(a => a.Accounts)
                .ToListAsync();

            var model = _mapper.Map<List<D.TeamWithAccount>>(teamswAccounts);
            return model;
        }

        public async Task<List<D.AccountForTeamDTO>> GetAccountsOfTeam(int id)
        {
            var accounts = await (from ACCOUNTS in DBCon.Accounts
                                  join TEAM in DBCon.Teams on ACCOUNTS.TeamId equals TEAM.TeamId
                                  where TEAM.TeamId == id
                                  select new D.AccountForTeamDTO
                                  {
                                      AccountId = ACCOUNTS.AccountId,
                                      AccountName=ACCOUNTS.AccountName,
                                  }).ToListAsync();
            return accounts;
        }

        public async Task<List<D.TeamDTO>> GetDropdownTeamsAsync()
        {
            var teams = await (from TEAMS in DBCon.Teams
                               where TEAMS.Status == true
                               select new D.TeamDTO
                               { 
                                   TeamId=TEAMS.TeamId,
                                   TeamName=TEAMS.TeamName,
                                   Status=TEAMS.Status
                               }).ToListAsync();
            return teams;
        }
        
    }
}
