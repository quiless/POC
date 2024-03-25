using System;
using MediatR;
using POC.Artifacts.Domain.Enumerators;

namespace POC.Artifacts.Domain.Models
{
    /// <summary>
    /// Modelo de notificação de domínio.
    /// </summary>
    public class DomainNotification : INotification
    {
        public DomainNotification(DomainNotificationType type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Tipo de notificação.
        /// </summary>
        public DomainNotificationType Type { get; protected set; }

        /// <summary>
        /// String com notificação.
        /// </summary>
        public string Value { get; protected set; }
    }
}

