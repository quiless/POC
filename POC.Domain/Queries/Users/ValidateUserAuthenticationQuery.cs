using System;
using MediatR;

namespace POC.Domain.Queries.Users
{
	public class ValidateUserAuthenticationQuery: IRequest<bool>
	{
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

