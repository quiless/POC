using System;
namespace POC.Artifacts.SQL.Repositories.Builders
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UseAutoMapBuilder : System.Attribute
    {
        public string SchemaName { get; private set; }
        public string TableName { get; private set; }
        public bool DisableFluentMap { get; private set; }

        public UseAutoMapBuilder(String schemaName, String tableName = "", bool disableFluentMap = false)
        {
            SchemaName = schemaName;
            TableName = tableName;
            DisableFluentMap = disableFluentMap;
        }

        public UseAutoMapBuilder(String tableName = "", bool disableFluentMap = false)
        {
            SchemaName = String.Empty;
            TableName = tableName;
            DisableFluentMap = disableFluentMap;
        }
    }

}

