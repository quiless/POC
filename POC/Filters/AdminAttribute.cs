using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using POC.Domain.Models.Context;

namespace POC.API.Filters
{

    /// <summary>
    /// Filtro de autorização para administradores
    /// </summary>
    public class AdminKeyAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Construtor do filtro de autorização para administradores
        /// </summary>
        public AdminKeyAttribute() : base(typeof(ValidateAdminFilter))
        {

        }

        private class ValidateAdminFilter : IAsyncActionFilter
        {
            private readonly ApplicationContextBase _applicationContext;

            public ValidateAdminFilter(ApplicationContextBase applicationContext)
            {
                _applicationContext = applicationContext;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {

                if (!_applicationContext.CustomIdentity.IsAdmin)
                {
                    context.Result = new UnauthorizedObjectResult("Você não possuí permissão para executar essa ação.");
                    return;
                }

                await next();
            }
        }
    }
}

