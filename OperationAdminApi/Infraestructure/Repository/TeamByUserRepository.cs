using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminDB.Data;
using OperationAdminRepository.Repository;
using Microsoft.EntityFrameworkCore;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class TeamByUserRepository : RepositoryBase<M.TeamByUser>
    {
        private readonly OperationAdminContext DBCon;
        public TeamByUserRepository(OperationAdminContext DBContext)
             : base(DBContext)
        {
            this.DBCon = DBContext;
        }

        public async Task<List<D.UserOnTeamDTO>> GetUsersOnTeam(int teamId) 
        {
            //List<D.TeamByUserDTO> users=await(from USERS in DBCon.Users
            //                                  join TEAMBYUSER in DBCon.TeamByUsers on USERS.UserId equals TEAMBYUSER.UserId
            //                                  join TEAM in DBCon.Teams on TEAMBYUSER.TeamId equals TEAM.TeamId
            //                                  where TEAMBYUSER.TeamId==teamId
            //                                  select new D.TeamByUserDTO
            //                                  {
            //                                      TeamByUserId=TEAMBYUSER.TeamByUserId,
            //                                      TeamId=TEAMBYUSER.TeamId,
            //                                      TeamName=TEAM.TeamName,
            //                                      UserId=TEAMBYUSER.UserId,
            //                                      UserName= USERS.FirstName + " " + USERS.LastName,
            //                                      StartDate=TEAMBYUSER.StartDate,
            //                                      EndDate=TEAMBYUSER.EndDate,
            //                                      DateRegister=TEAMBYUSER.DateRegister,
            //                                      Status=TEAMBYUSER.Status
            //                                  }).ToListAsync();

             List<D.UserOnTeamDTO> users = await(from USERS in DBCon.Users
                                                 join TEAMBYUSER in DBCon.TeamByUsers on USERS.UserId equals TEAMBYUSER.UserId
                                                 join TEAM in DBCon.Teams on TEAMBYUSER.TeamId equals TEAM.TeamId
                                                 where TEAMBYUSER.TeamId == teamId
                                                 select new D.UserOnTeamDTO
                                                 {
                                                     TeamByUserId = TEAMBYUSER.TeamByUserId,
                                                     TeamName = TEAM.TeamName,
                                                     UserId = TEAMBYUSER.UserId,
                                                     UserName = USERS.FirstName + " " + USERS.LastName,
                                                     StartDate = TEAMBYUSER.StartDate,
                                                     EndDate = TEAMBYUSER.EndDate,
                                                     DateRegister = TEAMBYUSER.DateRegister,
                                                     Status = TEAMBYUSER.Status
                                                 }).ToListAsync();
            return users;
        }

    }
}
