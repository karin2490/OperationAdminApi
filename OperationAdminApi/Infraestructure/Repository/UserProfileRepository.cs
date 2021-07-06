using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminDB.Data;
using Microsoft.EntityFrameworkCore;
using OperationAdminRepository.Repository;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class UserProfileRepository:RepositoryBase<M.UserProfile>
    {
        private readonly OperationAdminContext DBCon;
        public UserProfileRepository(OperationAdminContext DBContext)
            :base(DBContext)
        {
            this.DBCon = DBContext;
        }


        public async Task<D.UserProfileDTO>FindUserprofileByIdAsync(int userPorfId)
        {
            D.UserProfileDTO profile = await (from USERPROFILE in DBCon.UserProfiles
                                              join USERS in DBCon.Users on USERPROFILE.UserId equals USERS.UserId
                                              where USERPROFILE.ProfileId == userPorfId
                                              select new D.UserProfileDTO {
                                                  ProfileId=USERPROFILE.ProfileId,
                                                  UserId=USERPROFILE.UserId,
                                                  FirstName=USERS.FirstName,
                                                  LastName=USERS.LastName,
                                                  Email=USERS.Email,
                                                  EnglishLevel=USERPROFILE.EnglishLevel,
                                                  TechnicalSkills=USERPROFILE.TechnicalSkills,
                                                  LinkCv=USERPROFILE.LinkCv,
                                                  Status=USERPROFILE.Status
                                              }).FirstOrDefaultAsync();
            return profile;
                
        }
    }
}
