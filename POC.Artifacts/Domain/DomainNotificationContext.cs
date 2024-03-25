using System;
using POC.Artifacts.Domain.Enumerators;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Models;

namespace POC.Artifacts.Domain
{
    /// <summary>
    /// Notificações de domínio.
    /// </summary>
    public class DomainNotificationContext : IDomainNotificationContext
    {
        private readonly List<DomainNotification> _notifications;

        public DomainNotificationContext()
        {
            _notifications = new List<DomainNotification>();
        }

        /// <summary>
        /// Veja a documentação em <see cref="IDomainNotificationContext.HasErrorNotifications"/>
        /// </summary>
        public bool HasErrorNotifications
            => _notifications.Any(x => x.Type == DomainNotificationType.Error);

        /// <summary>
        /// Veja a documentação em <see cref="IDomainNotificationContext.NotErrorNotifications"/>
        /// </summary>
        public bool NotErrorNotifications
            => !HasErrorNotifications;

        /// <summary>
        /// Veja a documentação em <see cref="IDomainNotificationContext.NotifyError(string)"/>
        /// </summary>
        public void NotifyError(string message)
            => Notify(message, DomainNotificationType.Error);

        /// <summary>
        /// Veja a documentação em <see cref="IDomainNotificationContext.NotifyError(IEnumerable{string})"/>
        /// </summary>
        public void NotifyError(IEnumerable<string> messages)
        {
            foreach (var message in messages)
                Notify(message, DomainNotificationType.Error);
        }

        /// <summary>
        /// Veja a documentação em <see cref="IDomainNotificationContext.NotifySuccess(string)"/>
        /// </summary>
        public void NotifySuccess(string message)
            => Notify(message, DomainNotificationType.Success);

        /// <summary>
        /// Veja a documentação em <see cref="IDomainNotificationContext.GetErrorNotifications"/>
        /// </summary>
        public List<DomainNotification> GetErrorNotifications()
            => _notifications.Where(x => x.Type == DomainNotificationType.Error).ToList();

        /// <summary>
        /// Lança uma notificação.
        /// </summary>
        /// <param name="message">String com a notificação a ser lançada.</param>
        /// <param name="type">Tipo de notificação a ser lançada.</param>
        private void Notify(string message, DomainNotificationType type)
            => _notifications.Add(new DomainNotification(type, message));

        /// <summary>
        /// Limpa as notificações
        /// </summary>
        public void ClearNotifications() =>
            _notifications.Clear();
    }
}

