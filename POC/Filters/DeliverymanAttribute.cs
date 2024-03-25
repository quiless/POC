using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using POC.Domain.Models.Context;

namespace POC.API.Filters
{
	
    public class DeliverymanAttribute : TypeFilterAttribute
    {

        public DeliverymanAttribute() : base(typeof(ValidateDeliverymanFilter))
        {

        }

        private class ValidateDeliverymanFilter : IAsyncActionFilter
        {
            private readonly ApplicationContextBase _applicationContext;

            public ValidateDeliverymanFilter(ApplicationContextBase applicationContext)
            {
                _applicationContext = applicationContext;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {

                if (!_applicationContext.CustomIdentity.IsDeliveryman)
                {
                    context.Result = new UnauthorizedObjectResult("Você não possuí permissão para executar essa ação.");
                    return;
                }

                await next();
            }
        }
    }
}

