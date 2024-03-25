using System;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.API.Filters;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.Presentation;
using POC.Domain.Commands.Deliveryman;
using POC.Domain.Commands.Order;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Deliverymans;
using Swashbuckle.AspNetCore.Annotations;
using ThrottlR;

namespace POC.API.Controllers
{
    /// <summary>
    /// Executar rotinas em background.
    /// </summary>
    [ApiController, ApiVersion("1.0"), EnableThrottle]
    [Route("api/v{version:apiVersion}/background-services", Name = "Background Jobs")]
    public class BackgroundController : ApiControllerBase<BackgroundController>
    {
        public BackgroundController(
            ILogger<BackgroundController> logger,
            IMediator mediator,
            IDomainNotificationContext domainNotificationContext) : base(logger, mediator, domainNotificationContext)
        {

        }


        /// <summary>
        /// Processar fila de pedidos pendentes.
        /// </summary>
        /// <remarks>
        /// Busca entregadores para pedidos que ainda não foram aceitos.
        /// </remarks>
        /// <response code="200">
        /// Operação executada com sucesso!
        /// </response>
        /// <response code="400">
        /// Foi feita uma requisição inválida, erros de negócios foram detectados.
        /// </response>
        /// <response code="401">
        /// Os dados de autorização são inválidos.
        /// </response>
        /// <response code="405">
        /// O método HTTP utilizado não é permitido.
        /// </response>
        /// <response code="429">
        /// Limite de requisições excedido.
        /// </response>
        /// <response code="500">
        /// Falha interna do servidor.
        /// </response>
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(GenericCommandResult))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(List<ErrorResponse>))]
        [AdminKeyAttribute]
        [HttpPost("notify-deliveries-pending-orders"), MapToApiVersion("1.0")]
        public async Task<IActionResult> NotifyDeliveriesPendingOrdersAsync()
            => await TryNotificationCommand(new NotifyDeliveriesPendingOrdersCommand());


     

    }
}
