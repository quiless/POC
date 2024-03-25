﻿using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.API.Filters;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.Presentation;
using POC.Domain.Queries.Motorcycle;
using POC.Domain.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using ThrottlR;

namespace POC.API.Controllers
{
    /// <summary>
    /// Controladora de marcas de motos.
    /// </summary>
    [ApiController, ApiVersion("1.0"), EnableThrottle, Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/motorcycle-brand", Name = "MotorcycleBrand")]
    public class MotorcycleBrandController : ApiControllerBase<DeliverymanController>
    {
        public MotorcycleBrandController(
            ILogger<DeliverymanController> logger,
            IMediator mediator,
            IDomainNotificationContext domainNotificationContext) : base(logger, mediator, domainNotificationContext)
        {

        }

        /// <summary>
        /// Buscar marcas de motos.
        /// </summary>
        /// <remarks>
        /// Obtêm as marcas de motos que estão registradas na aplicação.
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
        [HttpGet("get-all"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMotorcycleBrandsAsync() =>
            await TrySendCommand(new GetMotorcycleBrandsQuery());


    }
}
