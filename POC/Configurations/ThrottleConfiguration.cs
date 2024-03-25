using System;
using ThrottlR;

namespace POC.API.Configurations
{
    ///<Summary>
    /// SETUP Throttle
    ///</Summary>
    public static class ThrottleConfiguration
    {
        ///<Summary>
        /// Política de throttle para evitar utilizações de robôs
        ///</Summary>
        public static WebApplicationBuilder AddThrottleConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddThrottlR(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                       .WithIpResolver()
                       .WithGeneralRule(TimeSpan.FromSeconds(1), 100)  
                       .WithGeneralRule(TimeSpan.FromMinutes(1), 2000) 
                       .WithGeneralRule(TimeSpan.FromHours(1), 50000); 
                });
            })
            .AddInMemoryCounterStore();

            return builder;
        }
    }
}

