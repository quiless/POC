using System;
using System.Linq.Expressions;
using System.Reflection;
using POC.Artifacts.SQL.Repositories.Builders;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Artifacts.SQL.Repositories.Models;
using Npgsql;
using Dommel;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace POC.Artifacts.SQL.Repositories
{

    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly SQLDbContextBase _dbContext;
        protected readonly string _tableName;
        private QueryFactory _db;
        private PostgresCompiler _compiler;

        protected RepositoryBase(SQLDbContextBase dbContext, string tableName = "")
        {
            _dbContext = dbContext;
            if (!string.IsNullOrEmpty(tableName))
                _tableName = tableName;
            else
                _tableName = typeof(TEntity).ToString();

            var TType = typeof(TEntity);
            _tableName = TType.Name.ToLower();

            var pTypeAttributes = (UseAutoMapBuilder[])TType.GetCustomAttributes(typeof(UseAutoMapBuilder), false);

            if (pTypeAttributes != null && pTypeAttributes.Length > 0
                && !string.IsNullOrEmpty(pTypeAttributes[0].SchemaName))
            {
                tableName = $"{pTypeAttributes[0].SchemaName}.{tableName}";
            }

            _compiler = new PostgresCompiler();

            _db = new QueryFactory(_dbContext.Connection, _compiler);

        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
          
            var query = _db.Query(_tableName).Where("id", "=", id).Where("isdeleted", "=", false);

            return await query.FirstOrDefaultAsync<TEntity>(transaction: _dbContext.Transaction);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            var query = _db.Query(_tableName);

            return await query.GetAsync<TEntity>(transaction: _dbContext.Transaction);
        }

        public virtual TEntity GetById(int id)
        {
            var query = _db.Query(_tableName).Where("id", "=", id);

            return query.FirstOrDefault<TEntity>(transaction: _dbContext.Transaction);
        }

        public virtual TEntity GetByUniqueId(Guid uuid)
        {
            var query = _db.Query(_tableName).Where("uniqueid", "=", uuid).Where("isdeleted", "=", false);

            return query.FirstOrDefault<TEntity>(transaction: _dbContext.Transaction);
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            var query = _db.Query(_tableName);

            var id = await _dbContext.Connection.InsertAsync(entity, transaction: _dbContext.Transaction);
            if (id != null)
            {
                SetObjectProperty("Id", Convert.ToInt32(id), entity);
                return Convert.ToInt32(id);
            }
            else
                return 0;
        }

        public virtual void DeleteByUniqueId(Guid uuid)
        {
            var query = _db
                .Query(_tableName)
                .Where("uniqueid", "=", uuid)
                .Update(new Dictionary<string, object> { { "isdeleted", true } }, transaction: _dbContext.Transaction);

            
        }

        private void SetObjectProperty(string propertyName, object value, object obj)
        {
            try
            {
                #pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
                #pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(obj, value, null);
                }
            }
            catch { }
        }

    }
}

