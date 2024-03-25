using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace POC.Domain.Models.Context
{
    public class ApplicationContext : ApplicationContextBase
    {
       
        private readonly IHttpContextAccessor _httpContextAccesor;

        public ApplicationContext(
            IHttpContextAccessor accessor)
        {
            _httpContextAccesor = accessor;
        }


        private ClaimsIdentity _Identity
        {
            get
            {

                #pragma warning disable CS8603 // Possível retorno de referência nula.
                #pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                return (ClaimsIdentity)_httpContextAccesor.HttpContext.User.Identity;
                #pragma warning restore CS8603 // Possível retorno de referência nula.
                #pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            }
        }


        public override bool IsAuthenticated
        {
            get
            {
                try
                {
                    if (_httpContextAccesor.HttpContext.User.Identity == null)
                        return false;

                    return _httpContextAccesor.HttpContext.User.Identity.IsAuthenticated;
                }
                catch
                {
                    return false;
                }
            }
        }


        private bool _IsAdmin()
        {
            if (!IsAuthenticated)
                return false;

            #pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
            return _ConvertStringToT<bool>(_Identity.FindFirst(Domain.Models.Environments.CustomClaimIdentity.IsAdmin).Value);
            #pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
        }

        private bool _IsDeliveryman()
        {
            if (!IsAuthenticated)
                return false;

            #pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
            return _ConvertStringToT<bool>(_Identity.FindFirst(Domain.Models.Environments.CustomClaimIdentity.IsDeliveryman).Value);
            #pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
        }

        private Guid _GetUserUUID()
        {
            if (!IsAuthenticated)
                return Guid.Empty;

            #pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                        return _ConvertStringToT<Guid>(_Identity.FindFirst(Domain.Models.Environments.CustomClaimIdentity.UserUniqueId).Value);
            #pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
        }

        private int _GetUserID()
        {
            if (!IsAuthenticated)
                return 0;

            #pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
            return _ConvertStringToT<Int32>(_Identity.FindFirst(Domain.Models.Environments.CustomClaimIdentity.UserId).Value);
            #pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
        }

        private T _ConvertStringToT<T>(string value)
        {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (typeof(T) == typeof(Guid))
                return (T)Convert.ChangeType(Guid.Parse(value), type); ;

            if (value == null)
                #pragma warning disable CS8603 // Possível retorno de referência nula.
                return default(T);
                #pragma warning restore CS8603 // Possível retorno de referência nula.
            return (T)Convert.ChangeType(value, type);
        }

        private string _GetToken() =>
            _httpContextAccesor.HttpContext.Request.Headers["Authorization"];

       
        public override TenantIdentity CustomIdentity
        {
            get
            {
                return new TenantIdentity
                {
                    IsAdmin = _IsAdmin(),
                    IsDeliveryman = _IsDeliveryman(),
                    UserUniqueId = _GetUserUUID(),
                    UserId = _GetUserID()
                };
            }
        }
    }
}

