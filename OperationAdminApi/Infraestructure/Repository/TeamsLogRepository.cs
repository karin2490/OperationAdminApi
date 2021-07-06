using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminDB.Data;
using Microsoft.EntityFrameworkCore;
using OperationAdminRepository.Repository;
using OperationAdminDB.Models;
using OpAdminRepository.Common.Cache;
using System;
using System.Collections.Generic;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminApi.CommonObjects.DTOs;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class TeamsLogRepository : RepositoryBase<M.TeamLog>
    {
        private readonly OperationAdminContext DBCon;
        public TeamsLogRepository(OperationAdminContext DBContext)
            : base(DBContext)
        {
            this.DBCon = DBContext;
        }

        public async Task InsertTeamLog(TeamLog activity)
        {
            activity.DateActivity = System.DateTime.Now;
            DBCon.Add(activity);
            await DBCon.SaveChangesAsync();
        }

        public async Task<List<D.TeamLogDTO>> GetAllTeamLogsAsync()
        {
            var teamLog = await DBCon.TeamLogs
                .Select(x => new D.TeamLogDTO()
                {
                    TeamLogId=x.TeamLogId,
                    Action=x.Action,
                    Module=x.Module,
                    UserId=x.UserId,
                    UserName=x.UserName,
                    Email=x.Email,
                    TeamId=x.TeamId,
                    TeamName=x.TeamName,
                    DataLog=x.DataLog,
                    DateActivity=x.DateActivity

                }).OrderByDescending(a => a.TeamLogId)
                .ToListAsync();
            return teamLog;
        }

       

        public TeamLog Teamlogger(UserCache user, string action = "", string module = "", TeamRequest team=null, string data = "")
        {
            var teamLog = new TeamLog()
            {
                Action = action,
                Module = module,
                UserId = user.UserId,
                UserName=user.FirtsName+" "+user.LastName,
                Email=user.Email,
                TeamId = team.TeamId,
                TeamName=team.TeamName,
                DataLog = data
            };

            return teamLog;
        }

        public async Task<List<D.TeamLogDTO>> GetTeamsLogByDateRange(DateTime? startDate, DateTime? endDate)
        {
            var teamLog = await DBCon.TeamLogs
                .Where(x => x.DateActivity >= startDate && x.DateActivity <= endDate.Value.AddDays(1))
                .Select(x => new D.TeamLogDTO()
                {
                    TeamLogId = x.TeamLogId,
                    Action = x.Action,
                    Module = x.Module,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.Email,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName,
                    DataLog = x.DataLog,
                    DateActivity = x.DateActivity
                })
                .OrderByDescending(a => a.TeamLogId)
                .ToListAsync();
            return teamLog;
        }

        public async Task<List<D.TeamLogDTO>> GetTeamsLogByTeamId(int TeamId)
        {
            var teamLog = await DBCon.TeamLogs
                .Where(x => x.TeamId==TeamId)
                .Select(x => new D.TeamLogDTO()
                {
                    TeamLogId = x.TeamLogId,
                    Action = x.Action,
                    Module = x.Module,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.Email,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName,
                    DataLog = x.DataLog,
                    DateActivity = x.DateActivity
                })
                .OrderByDescending(a => a.TeamLogId)
                .ToListAsync();
            return teamLog;
        }

        public async Task<List<D.TeamLogDTO>> GetTeamsLogByUserName(string userName)
        {
            var teamLog = await DBCon.TeamLogs
                .Where(x => x.UserName == userName)
                .Select(x => new D.TeamLogDTO()
                {
                    TeamLogId = x.TeamLogId,
                    Action = x.Action,
                    Module = x.Module,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.Email,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName,
                    DataLog = x.DataLog,
                    DateActivity = x.DateActivity
                })
                .OrderByDescending(a => a.TeamLogId)
                .ToListAsync();
            return teamLog;
        }

    }
}
