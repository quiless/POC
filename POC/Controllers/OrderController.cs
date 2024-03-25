using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.Presentation;
using POC.Domain.Queries.Deliverymans;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using ThrottlR;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Rental;
using POC.Domain.Commands.Order;
using POC.API.Filters;
using POC.Domain.Queries.Order;

namespace POC.API.Controllers
{
    /// <summary>
    /// Controladora de pedidos.
    /// </summary>
    [ApiController, ApiVersion("1.0"), EnableThrottle, Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/orders", Name = "Order")]
    public class OrderController : ApiControllerBase<OrderController>
    {
        public OrderController(
            ILogger<OrderController> logger,
            IMediator mediator,
            IDomainNotificationContext domainNotificationContext) : base(logger, mediator, domainNotificationContext)
        {

        }



        /// <summary>
        /// Cadastrar novo pedido
        /// </summary>
        /// <remarks>
        /// Registra um novo pedido para entrega. Notifica entregadores disponíveis.
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
        [HttpPost("register"), MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterOrderAsync([FromBody] RegisterOrderCommand request) =>
            await TrySendCommand(request);



        /// <summary>
        /// Aceitar pedido.
        /// </summary>
        /// <remarks>
        /// Altera o status do pedido para aceito. O motorista ficará indisponível até que o pedido seja concluído.
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
        [DeliverymanAttribute]
        [HttpPost("order-accepted"), MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterOrderAcceptedAsync(OrderAcceptedCommand request) =>
            await TrySendCommand(request);




        /// <summary>
        /// Finalizar pedido.
        /// </summary>
        /// <remarks>
        /// Encerra o pedido em aberto. O motorista ficará disponível assim que o pedido estiver encerrado.
        /// </remarks>
        /// <param name="request">Payload.</param>
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
        [DeliverymanAttribute]
        [HttpPost("order-finished"), MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterOrderFinishedAsync([FromBody] OrderFinishedCommand request) =>
            await TrySendCommand(request);





        /// <summary>
        /// Buscar pedidos.
        /// </summary>
        /// <remarks>
        /// Obtêm a lista de pedidos registrados. Há 04 parâmetros de filtros, não opcionais.
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
        /// TODO: Paginação
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(GenericCommandResult))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(List<ErrorResponse>))]
        [AdminKeyAttribute]
        [HttpGet("search-orders"), MapToApiVersion("1.0")]
        public async Task<IActionResult> SearchOrdersAsync([FromQuery] SearchOrdersQuery request) =>
            await TrySendCommand(request);



        /// <summary>
        /// Buscar notificações enviadas para uma ordem.
        /// </summary>
        /// <remarks>
        /// Retorna a lista de entregadores que receberam notificação de uma ordem.
        /// </remarks>
        /// <param name="request">Payload.</param>
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
        /// TODO: Paginação
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(GenericCommandResult))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(List<ErrorResponse>))]
        [AdminKeyAttribute]
        [HttpGet("get-order-notifications"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetOrderNotificationsAsync([FromQuery] GetOrderNotificationsQuery request) =>
           await TrySendCommand(request);



    }
}

