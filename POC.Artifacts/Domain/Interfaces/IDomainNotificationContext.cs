using System;
using POC.Artifacts.Domain.Models;

namespace POC.Artifacts.Domain.Interfaces
{
    /// <summary>
    /// Contrato de notificações de domínio.
    /// </summary>
	public interface IDomainNotificationContext
	{
        /// <summary>
        /// Flag que indica se existem notificações. 
        /// Retorna <c>true</c> caso existam notificações lançadas.
        /// </summary>
        bool HasErrorNotifications { get; }

        /// <summary>
        /// Lança uma notificação de erro.
        /// </summary>
        /// <param name="message">String com a notificação a ser lançada.</param>
        void NotifyError(string message);

        /// <summary>
        /// Lança notificações de erro através de uma lista.
        /// </summary>
        /// <param name="messages">Lista de mensagens a serem lançadas.</param>
        void NotifyError(IEnumerable<string> messages);

        /// <summary>
        /// Lança uma notificação de sucesso.
        /// </summary>
        /// <param name="message">String com a notificação a ser lançada.</param>
        void NotifySuccess(string message);

        /// <summary>
        /// Lista de notificações.
        /// </summary>
        /// <returns>Retorna lista do tipo <see cref="DomainNotification"/>.</returns>
        List<DomainNotification> GetErrorNotifications();

        /// <summary>
        /// Limpa as notificações
        /// </summary>
        void ClearNotifications();
    }
}

