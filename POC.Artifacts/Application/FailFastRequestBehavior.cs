using System;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using POC.Artifacts.Domain.Interfaces;

namespace POC.Artifacts.Application
{
    /// <summary>
    /// Valida os parâmetros de entrada das requests antes de seus respectivos handlers serem disparados.
    /// </summary>
    /// <typeparam name="TRequest">Instância de <typeparamref name="TRequest"/>.</typeparam>
    /// <typeparam name="Unit">Instância de <typeparamref name="Unit"/>.</typeparam>
    #pragma warning disable CS8714 // O tipo não pode ser usado como parâmetro de tipo no tipo ou método genérico. A nulidade do argumento de tipo não corresponde à restrição 'notnull'.
    public class FailFastRequestBehavior<TRequest, Unit> : IPipelineBehavior<TRequest, Unit>
    #pragma warning restore CS8714 // O tipo não pode ser usado como parâmetro de tipo no tipo ou método genérico. A nulidade do argumento de tipo não corresponde à restrição 'notnull'.
    {
        private readonly IEnumerable<IValidator> _validators;
        private readonly IDomainNotificationContext _notificationContext;

        /// <summary>
        /// Cria instância da classe.
        /// </summary>
        /// <param name="validators">Lista de validadores do <c>FluentValidation</c>.</param>
        /// <param name="notificationContext">Instância de <see cref="IDomainNotificationContext"/>.</param>
        public FailFastRequestBehavior(IEnumerable<IValidator<TRequest>> validators, IDomainNotificationContext notificationContext)
        {
            _validators = validators;
            _notificationContext = notificationContext;
        }

        /**
        public Task<Unit> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            var failures = _validators
               .Select(v => v.Validate(new ValidationContext<TRequest>(request)))
               .SelectMany(x => x.Errors)
               .Where(f => f != null)
               .ToList();

            return failures.Any() ? Notify(failures) : next();
        }**/

        /// <summary>
        /// Executa as validações e caso existam falhas, interrompe a pipeline e retorna as mensagens de erro.
        /// </summary>
        /// <param name="request">Solicitação recebida.</param>
        /// <param name="next">Delegate aguardável para a próxima ação na pipeline.</param>
        /// <param name="cancellationToken">Cancela a operação.</param>
        /// <returns>Tarefa aguardável retornado <typeparamref name="TResponse"/>.</returns>
        public Task<Unit> Handle(TRequest request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return next();

            var failures = _validators
                .Select(v => v.Validate(new ValidationContext<TRequest>(request)))
                .SelectMany(x => x.Errors)
                .Where(f => f != null)
                .ToList();

            return failures.Any() ? Notify(failures) : next();
        }

        /// <summary>
        /// Coloca as mensagens de validação na notificação do domínio.
        /// </summary>
        /// <param name="failures">Lista de mensagens.</param>
        /// <returns>Retorna instância de <typeparamref name="TResponse"/>.</returns>
        private Task<Unit> Notify(IEnumerable<ValidationFailure> failures)
        {
            var result = default(Unit);

            foreach (var failure in failures)
            {
                _notificationContext.NotifyError(failure.ErrorMessage);
            }

            #pragma warning disable CS8619 // A anulabilidade de tipos de referência no valor não corresponde ao tipo de destino.
            return Task.FromResult(result);
            #pragma warning restore CS8619 // A anulabilidade de tipos de referência no valor não corresponde ao tipo de destino.
        }
    }
}

