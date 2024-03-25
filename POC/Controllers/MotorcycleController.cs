using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.API.Filters;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.Presentation;
using POC.Domain.Queries.Deliverymans;
using POC.Domain.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using ThrottlR;
using POC.Domain.Commands;
using POC.Domain.Queries.Motorcycle;
using POC.Domain.Commands.Motorcycle;

namespace POC.API.Controllers
{

    /// <summary>
    /// Controladora de motos.
    /// </summary>
    [ApiController, ApiVersion("1.0"), EnableThrottle, Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/motorcycle", Name = "Motorcycle")]
    public class MotorcycleController : ApiControllerBase<DeliverymanController>
    {
        public MotorcycleController (
            ILogger<DeliverymanController> logger,
            IMediator mediator,
            IDomainNotificationContext domainNotificationContext) : base(logger, mediator, domainNotificationContext)
        {

        }

        /// <summary>
        /// Cadastrar nova moto.
        /// </summary>
        /// <remarks>
        /// Registra uma nova moto para locação.
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
        [HttpPost("register"), MapToApiVersion("1.0")]
        [AdminKeyAttribute]
        public Task<IActionResult> RegisterMotorcycleAsync([FromBody] RegisterMotorcycleCommand request) =>
            TrySendCommand(request);





        /// <summary>
        /// Buscar motos.
        /// </summary>
        /// <remarks>
        /// Obtêm as motos que estão registradas na aplicação.
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
        [HttpPost("get-all"), MapToApiVersion("1.0")]
        [AdminKeyAttribute]
        public Task<IActionResult> GetAllMotorcycleAsync() =>
            //TODO: adicionar paginação
            TrySendCommand(new GetMotorcyclesQuery());




        /// <summary>
        /// Pesquisar motos.
        /// </summary>
        /// <remarks>
        /// Obtêm as motos que estão registradas na aplicação aplicando filtros de pesquisa.
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
        [HttpPost("search"), MapToApiVersion("1.0")]
        public Task<IActionResult> GetMotorcycleByFilterAsync([FromQuery] SearchMotorcyclesQuery request) =>
            TrySendCommand(request);




        /// <summary>
        /// Remover moto cadastrada.
        /// </summary>
        /// <remarks>
        /// Desabilitada a moto informada. Motos desabilitadas não podem ser alugadas.
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
        [HttpDelete("remove-motorcycle/{motorcycleuniqueid}"), MapToApiVersion("1.0")]
        [AdminKeyAttribute]
        public async Task<IActionResult> RemoveMotorcycleAsync([FromRoute] Guid motorcycleuniqueid) =>
            await TrySendCommand(new RemoveMotorcycleCommand(motorcycleuniqueid));


    }
}

