using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;

namespace POC.Artifacts.Presentation
{
    /// <summary>
    /// Controller base com implementações de request e response usando o padrão Mediator.
    /// </summary>
    /// <typeparam name="T">Instância da controller que será extendida por <see cref="ApiControllerBase{T}"/>.</typeparam>
    public abstract class ApiControllerBase<T> : ControllerBase
    {
        public readonly ILogger<T> _logger;
        public readonly IMediator _mediator;
        public readonly IDomainNotificationContext _domainNotificationContext;

        protected ApiControllerBase(ILogger<T> logger, IMediator mediator, IDomainNotificationContext domainNotificationContext)
        {
            _logger = logger;
            _mediator = mediator;
            _domainNotificationContext = domainNotificationContext;
        }

        /// <summary>
        /// Processa a requet passando pelo padrão mediator retornando um status code.
        /// </summary>
        /// <typeparam name="TRequest">Intância do objeto command.</typeparam>
        /// <param name="command">Intância de <typeparamref name="TRequest"/>.</param>
        /// <param name="statusCode">Código a ser retornado em caso de sucesso, o padrão é 200. Veja mais em <seealso cref="StatusCodes"/>.</param>
        /// <returns>Retorna instância de <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> TrySendCommand<TRequest>(TRequest command, int? statusCode = null)
        {
            object value = new();

            try
            {
                value = await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(new { command = command.ToString() , exception = ex, parameter = JsonConvert.SerializeObject(command)}));
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Errors = new List<string> { "Ocorreu uma falha inesperada em nosso sistema. Estamos trabalhando na correção" }
                });
            }
            
            AddModelStateErrorsInNotifications();

            if (_domainNotificationContext.HasErrorNotifications)
            {
                return BadRequestDomainError();
            }

            if (value == null)
            {
                return StatusCode(
                    statusCode ?? StatusCodes.Status204NoContent,
                    new
                    {
                        Success = false,
                        Errors = new List<string> { "Nenhum registro encontrado." }
                    });
            } 

            return StatusCode(statusCode ?? StatusCodes.Status200OK, value);
        }



        /// <summary>
        /// Notificar comando pelo mediator
        /// </summary>
        /// <typeparam name="TRequest">Intância do objeto command.</typeparam>
        /// <param name="command">Intância de <typeparamref name="TRequest"/>.</param>
        /// <returns>Retorna instância de <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> TryNotificationCommand<TRequest>(TRequest command, int? statusCode = null)
        {
            object value = new();

            try
            {
                 await _mediator.Publish(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(new { command = command.ToString(), exception = ex, parameter = JsonConvert.SerializeObject(command) }));
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Errors = new List<string> { "Ocorreu uma falha inesperada em nosso sistema. Estamos trabalhando na correção" }
                });
            }

            AddModelStateErrorsInNotifications();

            if (_domainNotificationContext.HasErrorNotifications)
            {
                return BadRequestDomainError();
            }

            return StatusCode(statusCode ?? StatusCodes.Status200OK, value);
        }

        /// <summary>
        /// Trata o resultado.
        /// </summary>
        /// <typeparam name="T">Instância do objeto com o response obtido.</typeparam>
        /// <param name="result">Instância de <typeparamref name="TResult"/> com o response a ser validado.</param>
        /// <param name="statusCode">OPCIONAL. Código a ser retornado em caso de registros não encontrados, o padrão é 404. Veja mais em <seealso cref="StatusCodes"/>.</param>
        /// <returns>
        /// Retorna instância de <see cref="IActionResult"/> com status code <c>200 Ok</c> para sucesso ou
        /// status code <c>404 Not Found</c> caso o result seja <c>null</c> e não seja informado outro status code.
        /// </returns>
        protected IActionResult CustomResponse<TResult>(TResult result, int? statusCode = null)
        {
            if (_domainNotificationContext.HasErrorNotifications)
            {
                return BadRequestDomainError();
            }

            if (result == null)
            {
                return StatusCode(
                    statusCode ?? StatusCodes.Status404NotFound,
                    new
                    {
                        Success = false,
                        Errors = new List<string> { "Nenhum registro encontrado." }
                    });
            }

            return Ok(result);
        }

        /// <summary>
        /// Trata o resultado do TrySendCommand.
        /// </summary>
        /// <param name="result">Resultado do TrySendCommand</param>
        /// <param name="statusCode">OPCIONAL. Código a ser retornado em caso de registros não encontrados, o padrão é 404. Veja mais em <seealso cref="StatusCodes"/>.</param>
        /// Retorna instância de <see cref="IActionResult"/> com status code <c>200 Ok</c> para sucesso ou
        /// status code <c>404 Not Found</c> caso o result seja <c>null</c> e não seja informado outro status code.
        /// </returns>
        protected IActionResult CustomResponseHandlingByTrySendCommand(IActionResult result, int? statusCode = null)
        {
            var dataResult = (GenericCommandResult)((ObjectResult)result).Value;

            if (dataResult.Data == null || !dataResult.Success)
            {
                return StatusCode(
                    statusCode ?? StatusCodes.Status204NoContent,
                    new
                    {
                        Success = true,
                        Message = "Nenhum registro encontrado"
                    });
            }

            return Ok(dataResult);
        }

        /// <summary>
        /// Adiciona erros do model state na notificação de domínio.
        /// </summary>
        private void AddModelStateErrorsInNotifications()
        {
            if (ModelState.IsValid) return;

            var modelStateErrors = this.ModelState
                .SelectMany(modelState => modelState.Value.Errors)
                .Select(error => new { error.ErrorMessage })
                .ToList();

            foreach (var error in modelStateErrors)
            {
                _domainNotificationContext.NotifyError(error.ErrorMessage);
            }
        }

        /// <summary>
        /// Extrai os erros de domínio e os coloca em um retorno <c>400-BadRequest</c>.
        /// </summary>
        /// <returns>Retorna instância de <see cref="IActionResult"/>.</returns>
        private IActionResult BadRequestDomainError()
        {
            IEnumerable<string> errors = from x in _domainNotificationContext.GetErrorNotifications()
                                         select x.Value;
            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }
    }
}

