using System;
namespace POC.Artifacts.Domain.Responses.Interfaces
{
    /// <summary>
    /// Contrato de resultado genérico.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICommandResult<TEntity>
    {
        /// <summary>
        ///  Flag indicando sucesso <c>true</c> ou falha <c>false</c>.
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// Mensagem sobre o retorno.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Objeto retornado.
        /// </summary>
        TEntity Data { get; set; }
    }
}

