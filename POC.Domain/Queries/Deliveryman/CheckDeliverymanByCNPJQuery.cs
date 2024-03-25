using System;
using MediatR;

namespace POC.Domain.Queries.Deliverymans
{
	public class CheckDeliverymanByCNPJQuery: INotification
	{
		public CheckDeliverymanByCNPJQuery(string cnpj) =>
			this.CNPJ = cnpj;


		public string CNPJ { get; set; } = String.Empty;
    }
}

