using System;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MediatR;
using POC.Domain.Queries.Users;
using POC.Infrastructure.SQLRepositories.Interfaces;

namespace POC.API.Services
{
    public class ProfileService : IProfileService
    {
        private IMediator _mediator { get; set; }

        public ProfileService(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var _user = await _mediator.Send(new GetUserByUsernameQuery(context.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject).Value));

            if (_user != null)
            {
                context.IssuedClaims.Add(new Claim(Domain.Models.Environments.CustomClaimIdentity.IsAdmin, _user.IsAdmin.ToString()));
                context.IssuedClaims.Add(new Claim(Domain.Models.Environments.CustomClaimIdentity.IsDeliveryman, _user.IsDeliveryman.ToString()));
                context.IssuedClaims.Add(new Claim(Domain.Models.Environments.CustomClaimIdentity.UserUniqueId, _user.UniqueId.ToString()));
                context.IssuedClaims.Add(new Claim(Domain.Models.Environments.CustomClaimIdentity.UserId, _user.Id.ToString()));
            }
            else
            {
                
                throw new Exception("Sua conta está inativa.");
            }

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            await Task.CompletedTask;
        }

    }
}

