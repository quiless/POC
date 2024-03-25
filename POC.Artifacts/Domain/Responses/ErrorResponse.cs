using System;
using System.ComponentModel;

namespace POC.Artifacts.Domain.Responses
{
    public record ErrorResponse
    {
        /// <summary>
        /// Propriedade com valor que indica falha.
        /// </summary>
        [DefaultValue(false)]
        public bool Success { get; set; }

        /// <summary>
        /// Lista de mensagens de erro.
        /// </summary>
        public IEnumerable<string> Errors { get; set; }
    }
}

