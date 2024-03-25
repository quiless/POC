using System;
using Dapper;
using POC.Artifacts.Helpers;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces;

namespace POC.Infrastructure.SQLRepositories
{
    public class UserRepository : RepositoryBase<UserInfo>, IUserRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public UserRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }
        
        public async Task<bool> ValidateUser(string username, string password) =>
            await _dbContext.Connection
                .QuerySingleAsync<bool>(@"SELECT COUNT(1) > 0

                                                    FROM UserInfo

                                                    WHERE Username = @username
                                                    AND Password = @password
                                                    AND IsDeleted = false", new
                                                    {
                                                      username, password = password.Encrypt()
                                                    });


        public async Task<UserLoggedInfo> GetUser(string username)
        {
            var user = await _dbContext.Connection
                .QuerySingleOrDefaultAsync<UserLoggedInfo>(@"SELECT

                                                    UserInfo.Id,
                                                    UserInfo.Username,
                                                    UserInfo.UniqueId,
                                                    UserInfo.IsAdmin,
                                                    Deliveryman.Id as DeliverymanId

                                                    FROM UserInfo UserInfo

                                                    LEFT JOIN Deliveryman Deliveryman
                                                    ON UserInfo.Id = Deliveryman.UserId

                                                    WHERE UserInfo.Username = @username
                                                    AND UserInfo.IsDeleted = false", new
                {
                    username
                });


            return user;
        }

    }
}




