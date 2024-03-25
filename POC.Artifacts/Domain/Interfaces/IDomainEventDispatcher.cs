using System;
using MediatR;

namespace POC.Artifacts.Domain.Interfaces
{
    /// <summary>
    /// Contrato do despachante de eventos de domínios.
    /// </summary>
    public interface IDomainEventDispatcher
    {
        /// <summary>
        /// Listar eventos de domínios registrados.
        /// </summary>
        /// <returns>Retorna coleção de <see cref="INotification"/></returns>
        IReadOnlyCollection<INotification> GetDomainEvents();

        /// <summary>
        /// Adiciona uma notificação.
        /// </summary>
        /// <param name="eventItem">Instância do evento com dados da notificação.</param>
        void AddDomainEvent(INotification eventItem);

        /// <summary>
        /// Remove uma notificação.
        /// </summary>
        /// <param name="eventItem">Instância do evento com dados da notificação.</param>
        void RemoveDomainEvent(INotification eventItem);

        /// <summary>
        /// Retorna o total de notificações registradas.
        /// </summary>
        /// <returns>Retorna inteiro com o total de notificações registrada.</returns>
        int? TotalDomainEvents();

        /// <summary>
        /// Limpa todas as notificações registradas.
        /// </summary>
        void ClearDomainEvents();

        /// <summary>
        /// Despacha todas as notificações registradas.
        /// </summary>
        /// <returns></returns>
        Task DispatchDomainEventsAsync();
    }
}

