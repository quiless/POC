using System;
using System.Linq.Expressions;
using POC.Artifacts.SQL.Repositories.Models;

namespace POC.Artifacts.SQL.Repositories.Interfaces
{
    /// <summary>
    /// Contrato para o repositório de dados para o tipo TEntity.
    /// </summary>
    /// <typeparam name="TEntity">Instância da entidade do tipo <typeparamref name="TEntity"/>.</typeparam>
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Recupera um registro pelo seu identificador, assíncronamente.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Retorna instância da entidade do tipo <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetByIdAsync(int id);
        /// <summary>
        /// Recupera todos os regustros da entidade, assíncronamente.
        /// </summary>
        /// <returns>Retorna instância da entidade do tipo <typeparamref name="TEntity"/>.</returns>
        Task<IEnumerable<TEntity>> GetAsync();
        /// <summary>
        /// Insere um registo, assíncronamente.
        /// </summary>
        /// <param name="entity">Instância da entidade do tipo <typeparamref name="TEntity"/>.</param>
        /// <returns>Retorna <see cref="int"/> representando o identificador do registro inserido.</returns>
        Task<int> InsertAsync(TEntity entity);
        /// <summary>
        /// Recupera um registro pelo seu identificador, síncronamente.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Retorna instância da entidade do tipo <typeparamref name="TEntity"/>.</returns>
        TEntity GetById(int id);
        /// <summary>
        /// Recupera um registro pelo seu identificador, síncronamente.
        /// </summary>
        /// <param name="uuid">Identificador do tipo UUID do registro.</param>
        /// <returns>Retorna instância da entidade do tipo <typeparamref name="TEntity"/>.</returns>
        TEntity GetByUniqueId(Guid uuid);
        /// <summary>
        /// Deleção lógica um registro pelo seu identificador, síncronamente.
        /// </summary>
        /// <param name="uuid">Identificador do tipo UUID do registro.</param>
        /// <returns>Retorna instância da entidade do tipo <typeparamref name="TEntity"/>.</returns>
        void DeleteByUniqueId(Guid uuid);

    }
}

