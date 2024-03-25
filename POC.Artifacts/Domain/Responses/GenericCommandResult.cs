using System;
using Microsoft.AspNetCore.Http;
using POC.Artifacts.Domain.Responses.Interfaces;

namespace POC.Artifacts.Domain.Responses
{
    /// <summary>
    /// Modelo de resultado para <c>object</c>.
    /// </summary>
    public struct GenericCommandResult : ICommandResult<object>
    {
        /// <summary>
        /// Instância com flag de indicação de sucesso e message e data padrão.
        /// </summary>
        /// <param name="success"></param>
        public GenericCommandResult(bool success)
        {
            Success = success;
            Message = "";
            Data = null;
        }

        /// <summary>
        /// Instância do resultado.
        /// </summary>
        /// <param name="success">Flag que indica sucesso <c>true</c> ou falha <c>false</c>.</param>
        /// <param name="message">Mensagem sobre o retorno.</param>
        /// <param name="data">Objeto retornado.</param>
        public GenericCommandResult(bool success, string message, object data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        /// <summary>
        ///  Flag indicando sucesso <c>true</c> ou falha <c>false</c>.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensagem sobre o retorno.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Objeto retornado.
        /// </summary>
        public object Data { get; set; }


    }
}

