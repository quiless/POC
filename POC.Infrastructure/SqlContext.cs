using System;
using Microsoft.Extensions.Configuration;
using POC.Artifacts.SQL;

namespace POC.Infrastructure
{
    public class SqlContext : SQLDbContextBase
    {
        public SqlContext(IConfiguration configuration)
            : base(configuration.GetConnectionString("SqlConnectionString"))
        {

        }
    }
}

