using System;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;

namespace POC.Infrastructure.SQLRepositories.Interfaces
{
    public interface IUserRepository : IRepository<UserInfo>
    {
        Task<bool> ValidateUser(string username, string password);
        Task<UserLoggedInfo> GetUser(string username);
    }
}

