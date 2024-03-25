using System;
namespace POC.Artifacts.SQL.Transactions.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Inicia a transação no banco de dados.
        /// </summary>
        void Begin();

        /// <summary>
        /// Torna permanente o conjunto de alterações no banco de dados.
        /// </summary>
        void Commit();

        /// <summary>
        /// Coloca um bloco de códido no escopo de transação.
        /// </summary>
        /// <param name="act">Arrow Fucntion contendo o bloco de códido.</param>
        void Scope(Action<Action> act);

        /// <summary>
        /// Coloca um bloco de códido no escopo de transação, assíncronamente.
        /// </summary>
        /// <param name="act">Arrow Fucntion contendo o bloco de códido.</param>
        Task ScopeAsync(Func<Action, Task> act);

        /// <summary>
        /// Destroi o conjunto de alterações no banco de dados.
        /// </summary>
        void Rollback();
    }
}

