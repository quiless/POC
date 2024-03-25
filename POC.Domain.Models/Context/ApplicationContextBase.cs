using System;
namespace POC.Domain.Models.Context
{
    public abstract class ApplicationContextBase
    {
        public virtual TenantIdentity CustomIdentity
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual void SetCustomIdentity(TenantIdentity CustomIdentity)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsAuthenticated
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

