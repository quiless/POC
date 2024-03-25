using System;
using MediatR;
using POC.Domain.Queries.Users;
using POC.Domain.Models.Context;
using POC.Infrastructure.SQLRepositories;
using POC.Infrastructure.SQLRepositories.Interfaces;
using POC.Domain.Models.Entities;
using POC.Domain.Models.Aggregators;

namespace POC.Domain.QueriesHandlers.Users
{
	public class UserQueryHandler:
		IRequestHandler<ValidateUserAuthenticationQuery, bool>,
		IRequestHandler<GetUserByUsernameQuery, UserLoggedInfo>
	{

		private IUserRepository _userRepository;

		public UserQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

        public async Task<bool> Handle(ValidateUserAuthenticationQuery request, CancellationToken cancellationToken)
        {
			return await this._userRepository.ValidateUser(request.Username, request.Password);
        }

        public async Task<UserLoggedInfo> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
			return await this._userRepository.GetUser(request.Username);
        }
    }
}

