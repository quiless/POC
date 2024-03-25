using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Presentation;
using System.Net;
using System.Net.Mime;
using ThrottlR;
using Swashbuckle.AspNetCore.Annotations;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Queries;
using POC.Domain.Queries.Deliverymans;
using POC.Domain.Models.Entities;
using POC.API.Filters;
using POC.Domain.Commands.Motorcycle;
using POC.Domain.Commands.Deliveryman;
using static POC.API.Configurations.FileFormOperation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace POC.API.Controllers
{
    /// <summary>
    /// Controladora de entregadores.
    /// </summary>
    [ApiController, ApiVersion("1.0"), EnableThrottle]
    [Route("api/v{version:apiVersion}/deliveryman", Name = "Deliveryman")]
    public class DeliverymanController : ApiControllerBase<DeliverymanController>
    {
        public DeliverymanController(
            ILogger<DeliverymanController> logger,
            IMediator mediator,
            IDomainNotificationContext domainNotificationContext) : base(logger, mediator, domainNotificationContext)
        {

        }

        /// <summary>
        /// Cadastro de novo entregador.
        /// </summary>
        /// <remarks>
        /// Registra um novo entregador na aplicação.
        /// Parâmetros via formData para facilitar envio do arquivo no Swagger. Poderia utilizar base64, mas seria necessário realizar a conversão no momento de utilização.
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
        [AllowAnonymous]
        [HttpPost("register"), MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterDeliverymanAsync(
            [FromForm] RegisterDeliverymanCommand request) =>
            await TrySendCommand(request);


        /// <summary>
        /// Atualizar imagem da CNH do entregador que está autenticado.
        /// </summary>
        /// <remarks>
        /// Substitui a imagem da CNH atual, pela nova imagem.
        /// Parâmetros via formData para facilitar envio do arquivo no Swagger. Poderia utilizar base64, mas seria necessário realizar a conversão no momento de utilização.
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
        [HttpPut("refresh-deliveryman-driver-license-file"), MapToApiVersion("1.0")]
        [DeliverymanAttribute]
        public async Task<IActionResult> RefreshDeliverymanDriverLicenseFileAsync(
            [FromForm] RefreshDeliverymanDriverLicenseFileCommand request) =>
            await TrySendCommand(request);


        /// <summary>
        /// Buscar imagem da CNH.
        /// </summary>
        /// <remarks>
        /// Retorna a URL da imagem da CNH do entregador autenticado.
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
        [HttpGet("download-driver-license-file"), MapToApiVersion("1.0")]
        public async Task<IActionResult> DownloadDriverLicenseFileAsync(
            [FromQuery] GeDeliverymanDriverLicenseFileQuery request)
            => await TrySendCommand(request);


        /// <summary>
        /// Buscar informações do entregador.
        /// </summary>
        /// <remarks>
        /// Retorna as informações do entregador que está autenticado.
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
        [HttpGet("get-deliveryman-info"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDeliverymanInfoAsync() =>
            await TrySendCommand(new GetDeliverymanInfoQuery());
        

    }
}

