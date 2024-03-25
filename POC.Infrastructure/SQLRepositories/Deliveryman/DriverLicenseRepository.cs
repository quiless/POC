using System;
using Dapper;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces;
using POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman;

namespace POC.Infrastructure.SQLRepositories.Deliveryman
{
    public class DriverLicenseRepository : RepositoryBase<DriverLicenseType>, IDriverLicenseRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public DriverLicenseRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

    }
}

