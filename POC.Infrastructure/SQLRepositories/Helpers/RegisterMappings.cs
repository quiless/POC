using System;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using POC.Artifacts.SQL.Repositories;

namespace POC.Infrastructure.SQLRepositories.Helpers
{
    public class SqlRegisterMappings
    {
        private SqlRegisterMappings() { }

        public static void Register()
        {
            FluentMapper.Initialize(config =>
            {

                var assembly = AppDomain.CurrentDomain.Load(Domain.Models.Environments.DomainModelProjects);
                config.MappingAutoMapBuilderEntitiesInAssembly(assembly, "Entities");
                config.ForDommel();

            });
        }
    }
}

