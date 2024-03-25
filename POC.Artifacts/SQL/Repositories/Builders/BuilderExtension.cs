using System;
using System.Reflection;
using Dapper.FluentMap.Configuration;
using POC.Artifacts.SQL.Repositories.Builders;
using POC.Artifacts.SQL.Repositories.Extensions;

namespace POC.Artifacts.SQL.Repositories
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Localiza as entidades contidas no assembly informado e as registra.
        /// </summary>
        /// <param name="config">Métodos de configuração do Dapper.FluentMap.</param>
        /// <param name="assembly">Representa um assembly .NET.</param>
        /// <param name="entitiesFolders">
        /// (Opcional) Coleção de pastas dentro do assembly onde estão as entidades. 
        /// Informar o local das entidades aumenta o desempenho.
        /// </param>
        /// <returns>Retorna instância de <see cref="FluentMapConfiguration"/>.</returns>
        public static FluentMapConfiguration MappingAutoMapBuilderEntitiesInAssembly(this FluentMapConfiguration config, Assembly assembly, params string[] entitiesFolders)
        {
            var types = entitiesFolders.Length > 0
                ? assembly.GetExportedTypes().Where(a => entitiesFolders.Contains(
                    a.Namespace[a.Namespace.LastIndexOf('.')..].TrimStart('.')))
                : assembly.GetExportedTypes();

            foreach (var type in types)
            {
                if (type.IsClass && type.GetCustomAttributes(typeof(UseAutoMapBuilder), false).Length > 0)
                {
                    var autoMapBuilderGenericImplementation = typeof(AutoMapBuilderEntityMap<>).MakeGenericType(type);
                    var instance = Activator.CreateInstance(autoMapBuilderGenericImplementation);
                    MethodInfo addMapMethod = config.GetType().GetMethod("AddMap");
                    MethodInfo addMapMethodGenericType = addMapMethod.MakeGenericMethod(type);
                    addMapMethodGenericType.Invoke(config, new object[] { instance });
                }
            }

            return config;
        }
    }
}

