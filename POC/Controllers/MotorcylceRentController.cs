using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Presentation;
using System.Net.Mime;
using ThrottlR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Deliverymans;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using POC.API.Filters;
using POC.Domain.Commands.Rental;
using POC.Domain.Queries.Rental;

namespace POC.API.Controllers
{
	
    /// <summary>
    /// Controladora de locações de motos
    /// </summary>
    [ApiController, ApiVersion("1.0"), EnableThrottle, Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
    [DeliverymanAttribute]
    [Route("api/v{version:apiVersion}/rent", Name = "Rental")]
    public class MotorcylceRentController : ApiControllerBase<MotorcylceRentController>
    {
        public MotorcylceRentController(
            ILogger<MotorcylceRentController> logger,
            IMediator mediator,
            IDomainNotificationContext domainNotificationContext) : base(logger, mediator, domainNotificationContext)
        {

        }


        /// <summary>
        /// Planos de locação
        /// </summary>
        /// <remarks>
        /// Obtêm os planos de locação ativos no momento
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
        [HttpGet("get-rental-plans"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRentalPlansAsync() =>
            await TrySendCommand(new GetRentalMotorcyclePlansQuery());



        /// <summary>
        /// Alugar uma moto
        /// </summary>
        /// <remarks>
        /// Reserva uma moto para o entregador autenticado.
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
        [HttpPost("rent"), MapToApiVersion("1.0")]
        public async Task<IActionResult> RentMotorcycleAsync([FromBody] RentMotorcycleCommand request) =>
            await TrySendCommand(request);


        /// <summary>
        /// Finalizar locação.
        /// </summary>
        /// <remarks>
        /// Encerraa locação que está em aberto. O entregador poderá realizar nova alocação. A moto ficará disponível para nova alocação.
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
        [HttpPost("finalize-rent"), MapToApiVersion("1.0")]
        public async Task<IActionResult> FinalizeRentAsync() =>
            await TrySendCommand(new FinalizedRentMotorcycleCommand());


        /// <summary>
        /// Consultar valor de devolução.
        /// </summary>
        /// <remarks>
        /// Retorna o valor á ser pago pela locação na data desejada da entrega.
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
        [HttpGet("check-active-rental-price"), MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckRentalPriceAsync([FromQuery] CheckRentalPriceQuery request) =>
            await TrySendCommand(request);
    }
}

