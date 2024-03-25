using System;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.SQL.Repositories.Models;

namespace POC.Artifacts.Domain.Models
{
    /// <summary>
    /// Entidade de domínio.
    /// </summary>
    /// <typeparam name="TId">Identificador da entidade.</typeparam>
    public abstract class Entity<TId> : EntityBase
    {
        private int? _requestedHashCode;
        private TId _Id;
        public bool IsDeleted { get; set; }

        public virtual TId Id
        {
            get { return _Id; }
            protected set { _Id = value; }
        }

        protected Entity() { }
        protected Entity(IDomainEventDispatcher domainEventDispatcher) : base(domainEventDispatcher) { }

        /// <summary>
        /// Determina se foi criada uma nova instância do objeto com o operador <c>new</c>.
        /// </summary>
        /// <returns><see cref="true"/> se o objeto atual for <c>transient</c>, caso contrário, <see cref="false"/>.</returns>
        public bool IsTransient() =>
            EqualityComparer<TId>.Default.Equals(Id, default(TId));

       
        /// <summary>
        /// Determina se o objeto informado é igual ao objeto atual.
        /// </summary>
        /// <param name="obj">Instância do objeto informado.</param>
        /// <returns><see cref="true"/> se o objeto informado for igual ao objeto atual, caso contrário, <see cref="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TId>))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity<TId> item = (Entity<TId>)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return EqualityComparer<TId>.Default.Equals(Id, item.Id);
        }

        /// <summary>
        /// Obtêm o hash para a instância do objeto atual.
        /// </summary>
        /// <returns>Retorna o hash do objeto.</returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; 

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }
    }
}

