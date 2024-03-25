using System;
namespace POC.Artifacts.SQL.Repositories.Builders
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoMapBuilder : System.Attribute
    {
        public enum Type
        {
            Key,
            Identity,
            NotIdentity,
            OnlyInsert,
            OnlyUpdate,
            Ignore,
            Computed,
            Both
        }

        public IList<Type> typesBuilder { get; set; }
        public string columnName { get; set; }

        public AutoMapBuilder(Type type, string columnName = "")
        {
            typesBuilder = new List<Type>();
            typesBuilder.Add(type);
            this.columnName = columnName;
        }

        
        public AutoMapBuilder(string columnName, params Type[] types)
        {
            typesBuilder = types.ToList<Type>();
            this.columnName = columnName;
        }
    }
}

