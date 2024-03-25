using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace POC.Artifacts.SQL
{
    /// <summary>
    /// Contexto de conexão com o SQL.
    /// </summary>
    public abstract class SQLDbContextBase : IDisposable
    {
        #pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        protected SQLDbContextBase(string connectionString)
        #pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        {
            SessionId = Guid.NewGuid();
            Connection = new NpgsqlConnection(connectionString);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Fecha a conexão com o banco de dados.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            Connection?.Close();
            Connection?.Dispose();
        }

        /// <summary>
        /// Código de rastreamento da sessão.
        /// </summary>
        public Guid SessionId { get; private set; }

        /// <summary>
        /// Gerencia a conexão com o banco de dados.
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        /// Gerencia as transações no banco de dados.
        /// </summary>
        public IDbTransaction Transaction { get; set; }
    }
}

