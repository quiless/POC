using System;
using MediatR;
using POC.Artifacts.Domain.Interfaces;

namespace POC.Artifacts.SQL.Repositories.Models
{
    
    public abstract class EntityBase
    {
        private IDomainEventDispatcher _domainEventDispatcher { get; set; }

        #pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        public EntityBase() { }
        #pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

        public EntityBase(IDomainEventDispatcher domainEventDispatcher)
        {
            this._domainEventDispatcher = domainEventDispatcher;
        }

        
        public void SetDomainEventDispatcher(IDomainEventDispatcher domainEventDispatcher)
        {
            this._domainEventDispatcher = domainEventDispatcher;
        }

        public void AddDomainEvent(INotification eventItem)
        {
            if (this._domainEventDispatcher == null)
                throw new Exception("Domínio inválido.");

            this._domainEventDispatcher.AddDomainEvent(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            if (this._domainEventDispatcher == null)
                throw new Exception("Domínio inválido.");

            this._domainEventDispatcher.RemoveDomainEvent(eventItem);
        }

        public void ClearDomainEvents()
        {
            if (this._domainEventDispatcher == null)
                throw new Exception("Domínio inválido.");

            this._domainEventDispatcher.ClearDomainEvents();
        }
    }
}

