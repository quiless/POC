using System;
using MediatR;
using POC.Domain.Models.Aggregators;

namespace POC.Domain.Queries.Users
{
	public class GetUserByUsernameQuery: IRequest<UserLoggedInfo>
	{
		public GetUserByUsernameQuery(string _username) =>
			this.Username = _username;

        public string Username { get; set; }
	}
}

