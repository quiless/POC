using System;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MediatR;
using System.Security.Claims;
using POC.Domain.Queries;
using POC.Domain.Queries.Users;

namespace POC.API.Security
{
    /// <summary>
    /// Autenticador Identity4
    /// </summary>
    public class ResourceOwnerPasswordValidatorCustom : IResourceOwnerPasswordValidator
    {
        private readonly int _expirationTimeInMinutes;
        private readonly IMediator _mediator;

        /// <summary>
        /// Construtor da classe autenticadora
        /// </summary>
        public ResourceOwnerPasswordValidatorCustom(
            IConfiguration configuration,
            IMediator mediator)
        {
            _expirationTimeInMinutes = configuration.GetValue<int>("VerificationCodeSettings:ExpirationTimeInMinutes");
            _mediator = mediator;
        }


        /// <summary>
        /// Valida a conexão do usuário -> Métodos password
        /// </summary>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            Dictionary<string, object> customResponse = new Dictionary<string, object>()
            {
                { "issuer", "Jefferson Quiles" }
            };

            var _validated = await _mediator.Send(new ValidateUserAuthenticationQuery()
            {
                Username = context.UserName,
                Password = context.Password
            });

            if (!_validated)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    "Usuário e/ou senha inválido.",
                    customResponse);
                return;
            }
            
            context.Result = new GrantValidationResult(
                subject: context.UserName,
                authenticationMethod: "custom",
                customResponse: customResponse);


        }
    }
}   

